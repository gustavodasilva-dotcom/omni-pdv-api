using MongoDB.Driver;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.Abstractions.Infra;

public interface IMongoContext
{
    IMongoCollection<Product> Products { get; }
    IMongoCollection<Manufacturer> Manufacturers { get; }
    IMongoCollection<Sale> Sales { get; }
    IMongoCollection<Client> Clients { get; }
}