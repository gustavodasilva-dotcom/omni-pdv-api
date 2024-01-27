using OmniePDV.Core.Entities;

namespace OmniePDV.Core.Models.Requests;

public class SendReceiptEmailMessageRequest(Sale sale, string email)
{
    public Sale Sale { get; private set; } = sale;
    public string Email { get; private set; } = email.Trim();
}
