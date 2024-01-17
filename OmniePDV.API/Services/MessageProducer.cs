using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OmniePDV.API.Options;
using OmniePDV.API.Services.Interfaces;
using RabbitMQ.Client;

namespace OmniePDV.API.Services;

public sealed class MessageProducer(IOptions<RabbitMQOptions> options) : IMessageProducer
{
    private readonly RabbitMQOptions _rabbitMQOptions = options.Value;

    public void SendMessage<T>(T message)
    {
        ConnectionFactory factory = new()
        {
            HostName = _rabbitMQOptions.Authentication.HostName,
            UserName = _rabbitMQOptions.Authentication.UserName,
            Password = _rabbitMQOptions.Authentication.Password,
            VirtualHost = _rabbitMQOptions.Authentication.VirtualHost
        };

        IConnection connection = factory.CreateConnection();

        using var channel = connection.CreateModel();
        channel.QueueDeclare(_rabbitMQOptions.Queue, durable: true, exclusive: false);

        string json = JsonSerializer.Serialize(message);
        byte[] body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: _rabbitMQOptions.Queue,
            body: body
        );        
    }
}