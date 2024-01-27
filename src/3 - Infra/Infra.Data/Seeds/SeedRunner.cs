using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OmniePDV.Core.Configurations;

namespace OmniePDV.Infra.Data.Seeds;

public static class SeedRunner
{
    public static async Task RunAsync(ConfigurationManager configuration)
    {
        MongoDBSettings options = configuration
            .GetSection(MongoDBSettings.Position)
            .Get<MongoDBSettings>() ??
            throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                MongoDBSettings.Position));

        MongoClient client = new(options.ConnectionString);
        IMongoDatabase database = client.GetDatabase(options.Database);

        await new DefaultClientSeed(configuration).ExecuteAsync(database);
    }
}