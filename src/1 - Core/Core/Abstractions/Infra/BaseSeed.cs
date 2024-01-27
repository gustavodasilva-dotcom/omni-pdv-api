using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace OmniePDV.Core.Abstractions.Infra;

public abstract class BaseSeed<T>
{
    protected readonly ILogger<T> _logger;
    protected readonly IConfiguration _configuration;

    public BaseSeed(IConfiguration configuration)
    {
        _configuration = configuration;

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        _logger = loggerFactory.CreateLogger<T>();
    }

    public virtual Task ExecuteAsync(IMongoDatabase database) =>
        throw new NotImplementedException();
}
