using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;

namespace OmniePDV.Infra.Data.Seeds;

public sealed class DefaultClientSeed(IConfiguration configuration)
    : BaseSeed<DefaultClientSeed>(configuration)
{
    public override async Task ExecuteAsync(IMongoDatabase database)
    {
        try
        {
            _logger.LogInformation("{date} - Running seed {seed}", DateTime.Now, nameof(ExecuteAsync));

            DefaultClientSettings options = _configuration
                .GetSection(DefaultClientSettings.Position)
                .Get<DefaultClientSettings>() ??
                throw new Exception(string.Format("Section {0} not found at the environmental appsettings.json",
                    DefaultClientSettings.Position));

            var collection = database.GetCollection<Client>(nameof(Client).Pluralize());

            Client client = await collection.FindSync(c => c.Name.Trim().ToLower()
                .Equals(options.Name.Trim().ToLower())).FirstOrDefaultAsync();
            if (client == null)
            {
                client = new(
                    name: options.Name,
                    ssn: options.SSN,
                    birthday: DateTime.Now,
                    email: null,
                    active: true
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