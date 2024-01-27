using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Clients.Commands.DeleteClient;

internal sealed class DeleteClientCommandHandler(IMongoContext mongoContext)
    : IRequestHandler<DeleteClientCommand>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        if (request.ID.Equals(Guid.Empty))
            throw new BadRequestException(string.Format("Invalid id {0}", request.ID));
        
        if (!await _mongoContext.Clients.Find(m => m.UID == request.ID).AnyAsync())
            throw new BadRequestException(string.Format("Client not found with the id {0}", request.ID));
        
        if (await _mongoContext.Sales.Find(s => s.Client != null && s.Client.UID.Equals(request.ID)).AnyAsync())
            throw new BadRequestException("This client cannot be deleted, because it has sales history");

        await _mongoContext.Clients.DeleteOneAsync(m => m.UID == request.ID);
    }
}
