using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;

namespace OmniePDV.Infra.Data;

public class MongoContext : IMongoContext
{
    private readonly MongoDBSettings _options;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    
    public MongoContext(IOptions<MongoDBSettings> options)
    {
        _options = options.Value;
        _client = new MongoClient(_options.ConnectionString);
        _database = _client.GetDatabase(_options.Database);
    }

    public IMongoCollection<Product> Products
    {
        get
        {
            return _database.GetCollection<Product>(nameof(Products));
        }
    }

    public IMongoCollection<Manufacturer> Manufacturers
    {
        get
        {
            return _database.GetCollection<Manufacturer>(nameof(Manufacturers));
        }
    }

    public IMongoCollection<Sale> Sales
    {
        get
        {
            return _database.GetCollection<Sale>(nameof(Sales));
        }
    }

    public IMongoCollection<Client> Clients
    {
        get
        {
            return _database.GetCollection<Client>(nameof(Clients));
        }
    }
}
