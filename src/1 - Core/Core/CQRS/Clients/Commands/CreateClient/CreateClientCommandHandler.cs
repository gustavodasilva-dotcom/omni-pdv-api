using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Clients.Commands.CreateClient;

internal sealed class CreateClientCommandHandler(
    IMongoContext mongoContext,
    IOptions<DefaultClientSettings> defaultClientSettings) : IRequestHandler<CreateClientCommand, Client>
{
    private readonly IMongoContext _mongoContext = mongoContext;
    private readonly DefaultClientSettings _defaultClientSettings = defaultClientSettings.Value;

    public async Task<Client> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        Client defaultClient = await _mongoContext.Clients
            .Find(c => c.Name.Equals(_defaultClientSettings.Name))
            .FirstOrDefaultAsync();
        if (request.Name.Trim().ToLower().Equals(defaultClient.Name.Trim().ToLower()))
            throw new ConflictException(string.Format("The name {0} is reserved", defaultClient.Name));

        Client client = await _mongoContext.Clients
            .Find(c => c.SSN.Equals(request.SSN))
            .FirstOrDefaultAsync();
        if (client is not null)
            throw new ConflictException(string.Format("There's already a client with the SSN {0}", request.SSN));

        client = new(
            name: request.Name,
            ssn: request.SSN,
            birthday: request.Birthday,
            email: request.Email,
            active: request.Active
        );
        await _mongoContext.Clients.InsertOneAsync(client);

        return client;
    }
}