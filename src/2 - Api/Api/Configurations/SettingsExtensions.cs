using OmniePDV.Core.Configurations;

namespace OmniePDV.Api.Configurations;

public static class SettingsExtensions
{
    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DefaultClientSettings>(configuration.GetSection(DefaultClientSettings.Position));
        services.Configure<MongoDBSettings>(configuration.GetSection(MongoDBSettings.Position));
        services.Configure<RabbitMQSettings>(configuration.GetSection(RabbitMQSettings.Position));
    }
}