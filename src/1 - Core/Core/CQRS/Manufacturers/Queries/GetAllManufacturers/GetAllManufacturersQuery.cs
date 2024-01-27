using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Manufacturers.Queries.GetAllManufacturers;

public sealed record GetAllManufacturersQuery : IRequest<List<Manufacturer>>;
