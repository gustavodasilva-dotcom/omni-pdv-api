namespace OmniePDV.API.Data.Entities;

public sealed class SaleProduct(Guid UID, int Order, double Quantity, Product Product) : Entity(UID)
{
    public int Order { get; private set; } = Order;
    public double Quantity { get; private set; } = Quantity;
    public Product Product { get; private set; } = Product;
    public bool Deleted { get; private set; } = false;

    public void DeleteProduct() => Deleted = true;
}
