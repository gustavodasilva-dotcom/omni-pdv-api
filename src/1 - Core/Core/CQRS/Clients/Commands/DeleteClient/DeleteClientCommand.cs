using MediatR;

namespace OmniePDV.Core.CQRS.Clients.Commands.DeleteClient;

public record class DeleteClientCommand(Guid ID) : IRequest;
