﻿using Humanizer;
using MongoDB.Driver;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Options.Data;
using OmniePDV.API.Options.Global;

namespace OmniePDV.API.Data.Seeders;

public sealed class SeedRunner
{
    private static ILogger<SeedRunner> _logger;
    private static ConfigurationManager _configuration;

    public static async Task ExecuteAsync(ConfigurationManager configuration)
    {
        _configuration = configuration;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        _logger = loggerFactory.CreateLogger<SeedRunner>();

        _logger.LogInformation($"{DateTime.Now} - Starting to run seeds");

        MongoDBOptions options = _configuration.GetSection(MongoDBOptions.Position).Get<MongoDBOptions>() ??
            throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                MongoDBOptions.Position));

        MongoClient client = new(options.ConnectionString);
        IMongoDatabase database = client.GetDatabase(options.Database);

        await SeedDefaultClient(database);
    }

    static async Task SeedDefaultClient(IMongoDatabase database)
    {
        try
        {
            GlobalOptions options = _configuration.GetSection(GlobalOptions.Position).Get<GlobalOptions>() ??
                throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                    GlobalOptions.Position));

            var collection = database.GetCollection<Client>(nameof(Client).Pluralize());

            Client client = await collection.FindSync(c => c.Name.Trim().ToLower()
                .Equals(options.Client.Default.Name.Trim().ToLower())).FirstOrDefaultAsync();
            if (client == null)
            {
                client = new(
                    Name: options.Client.Default.Name,
                    SSN: options.Client.Default.SSN,
                    Birthday: DateTime.Now,
                    Active: true
                );
                await collection.InsertOneAsync(client);
            }
            else
            {
                client.SetName(options.Client.Default.Name);
                client.SetSSN(options.Client.Default.SSN);
                await collection.ReplaceOneAsync(c => c.UID.Equals(client.UID), client);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"{DateTime.Now} - Error running default client seeder - {e.Message}");
        }
    }
}