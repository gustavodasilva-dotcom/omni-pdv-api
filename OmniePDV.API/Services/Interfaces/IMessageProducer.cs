namespace OmniePDV.API.Services.Interfaces;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}