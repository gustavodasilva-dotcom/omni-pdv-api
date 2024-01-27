using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Clients.Commands.UpdateClient;

internal sealed class UpdateClientCommandHandler(
    IMongoContext mongoContext,
    IOptions<DefaultClientSettings> defaultClientSettings) : IRequestHandler<UpdateClientCommand, Client>
{
    private readonly IMongoContext _mongoContext = mongoContext;
    private readonly DefaultClientSettings _defaultClientSettings = defaultClientSettings.Value;

    public async Task<Client> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        Client defaultClient = await _mongoContext.Clients
            .Find(c => c.Name.Equals(_defaultClientSettings.Name))
            .FirstOrDefaultAsync();
        if (request.Name.Trim().ToLower().Equals(defaultClient.Name.Trim().ToLower()))
            throw new ConflictException(string.Format("The name {0} is reserved", defaultClient.Name));

        Client client = await _mongoContext.Clients
            .Find(m => m.UID.Equals(request.ID))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No client was found with the id {0}", request.ID));

        client.SetName(request.Name);
        client.SetSSN(request.SSN);
        client.SetBirthday(request.Birthday);
        client.SetEmail(request.Email);
        client.SetActive(request.Active);

        await _mongoContext.Clients.ReplaceOneAsync(c => c.UID.Equals(request.ID), client);

        return client;
    }
}
