using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Options;

namespace OmniePDV.API.Data;

public interface IMongoContext
{
    IMongoCollection<Product> Products { get; }
    IMongoCollection<Manufacturer> Manufacturers { get; }
    IMongoCollection<Sale> Sales { get; }
}

public class MongoContext : IMongoContext
{
    private readonly MongoDBOptions _options;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoDBOptions> options)
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
}
