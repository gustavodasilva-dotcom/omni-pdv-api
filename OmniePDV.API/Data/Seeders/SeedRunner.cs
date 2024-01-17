using Humanizer;
using MongoDB.Driver;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Options;

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

        _logger.LogInformation("{date} - Starting to run seeds", DateTime.Now);

        MongoDBOptions options = _configuration
            .GetSection(MongoDBOptions.Position)
            .Get<MongoDBOptions>() ??
            throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                MongoDBOptions.Position));

        MongoClient client = new(options.ConnectionString);
        IMongoDatabase database = client.GetDatabase(options.Database);

        await SeedDefaultClient(database);

        _logger.LogInformation("{date} - All seeds ran successfully", DateTime.Now);
    }

    static async Task SeedDefaultClient(IMongoDatabase database)
    {
        try
        {
            _logger.LogInformation("{date} - Running seed {seed}", DateTime.Now, nameof(SeedDefaultClient));

            DefaultClientOptions options = _configuration
                .GetSection(DefaultClientOptions.Position)
                .Get<DefaultClientOptions>() ??
                throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                    DefaultClientOptions.Position));

            var collection = database.GetCollection<Client>(nameof(Client).Pluralize());

            Client client = await collection.FindSync(c => c.Name.Trim().ToLower()
                .Equals(options.Name.Trim().ToLower())).FirstOrDefaultAsync();
            if (client == null)
            {
                client = new(
                    Name: options.Name,
                    SSN: options.SSN,
                    Birthday: DateTime.Now,
                    Active: true
                );
                await collection.InsertOneAsync(client);
            }
            else
            {
                client.SetName(options.Name);
                client.SetSSN(options.SSN);
                await collection.ReplaceOneAsync(c => c.UID.Equals(client.UID), client);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{date} - Error running default client seeder - {message}",
                DateTime.Now, e.Message);
        }
    }
}
