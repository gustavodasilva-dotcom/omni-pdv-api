namespace OmniePDV.API.Data.Entities;

public sealed class SaleProduct(Guid UID, double Quantity, Product Product) : Entity(UID)
{
    public double Quantity { get; private set; } = Quantity;
    public Product Product { get; private set; } = Product;
}
