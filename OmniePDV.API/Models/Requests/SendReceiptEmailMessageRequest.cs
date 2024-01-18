using OmniePDV.API.Data.Entities;

namespace OmniePDV.API.Models.Requests;

public class SendReceiptEmailMessageRequest(Sale sale, string email)
{
    public Sale Sale { get; private set; } = sale;
    public string Email { get; private set; } = email.Trim();
}
