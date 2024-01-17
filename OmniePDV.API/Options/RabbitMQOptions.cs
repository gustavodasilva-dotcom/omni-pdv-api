namespace OmniePDV.API.Options;

public sealed class AuthenticationRabbitMQOptions
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
}

public sealed class RabbitMQOptions
{
    public const string Position = "RabbitMQ";

    public string Queue { get; set; } = string.Empty;
    public AuthenticationRabbitMQOptions Authentication { get; set; }
}