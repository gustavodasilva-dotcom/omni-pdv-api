using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Abstractions.Services;
using OmniePDV.Core.Services;
using OmniePDV.Infra.Data;

namespace OmniePDV.Api.Configurations;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoContext, MongoContext>();
        services.AddSingleton<IMessageProducer, MessageProducer>();
    }
}