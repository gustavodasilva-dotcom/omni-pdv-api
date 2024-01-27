namespace OmniePDV.Core.Abstractions.Services;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}