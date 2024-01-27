namespace OmniePDV.Core.Configurations;

public sealed class AuthenticationRabbitMQOptions
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
}

public sealed class RabbitMQSettings
{
    public const string Position = "RabbitMQ";

    public string Queue { get; set; }
    public AuthenticationRabbitMQOptions Authentication { get; set; }
}