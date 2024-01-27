using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Abstractions.Services;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;
using OmniePDV.Core.Models.Requests;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.SendSaleReceiptToEmail;

internal sealed class SendSaleReceiptToEmailCommandHandler(
    IMongoContext mongoContext,
    IMessageProducer messageProducer,
    IOptions<DefaultClientSettings> defaultClientSettings) : IRequestHandler<SendSaleReceiptToEmailCommand>
{
    private readonly IMongoContext _mongoContext = mongoContext;
    private readonly IMessageProducer _messageProducer = messageProducer;
    private readonly DefaultClientSettings _defaultClientSettings = defaultClientSettings.Value;

    public async Task Handle(SendSaleReceiptToEmailCommand request, CancellationToken cancellationToken)
    {
        if (request.SaleID.Equals(Guid.Empty))
            throw new BadRequestException("Invalid sale_id");

        Sale sale = await _mongoContext.Sales
            .Find(s => s.UID.Equals(request.SaleID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException($"There's no sale with the id {request.SaleID}");

        if ((sale.Client.SSN.Equals(_defaultClientSettings.SSN)
            || string.IsNullOrEmpty(sale.Client.Email))
            && string.IsNullOrEmpty(request.Email))
            throw new BadRequestException("Email cannot be null or empty");

        if (!sale.Client.SSN.Equals(_defaultClientSettings.SSN)
            && string.IsNullOrEmpty(sale.Client.Email))
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new BadRequestException("Email cannot be null or empty");

            Client client = await _mongoContext.Clients
                .Find(c => c.UID.Equals(sale.Client.UID))
                .FirstOrDefaultAsync();

            client.SetEmail(request.Email);
            await _mongoContext.Clients.ReplaceOneAsync(c => c.UID.Equals(client.UID), client);

            sale.SetClient(client);
            await _mongoContext.Sales.ReplaceOneAsync(c => c.UID.Equals(sale.UID), sale);
        }

        SendReceiptEmailMessageRequest message = new(
            sale: sale,
            email: request.Email
        );
        _messageProducer.SendMessage(message);
    }
}
