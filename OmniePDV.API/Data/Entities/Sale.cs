using OmniePDV.API.Entities;

namespace OmniePDV.API.Data.Entities;

public sealed class Sale(
    Guid UID,
    double Discount,
    double Subtotal,
    double Total,
    List<SaleProduct> Products) : Entity(UID)
{
    public double Subtotal { get; private set; } = Subtotal;
    public double Discount { get; private set; } = Discount;
    public double Total { get; private set; } = Total;
    public List<SaleProduct> Products { get; private set; } = Products;
    public SaleStatus Status { get; private set; } = SaleStatus.Open;
    public DateTime SaleDate { get; private set; } = DateTime.Now;

    public void AddProduct(SaleProduct product) => Products.Add(product);
    public void UpdateSubtotal(double Subtotal) => this.Subtotal += Subtotal;
    public void AddDiscount(double Discount) => this.Discount = Discount;
    public void UpdateTotal() => Total = Subtotal - Discount;

    public void CloseSale()
    {
        if (Status != SaleStatus.Open)
            throw new Exception("It's not possible to close an already closed sale");
        Status = SaleStatus.Closed;
    }
    public void CancelSale()
    {
        if (Status != SaleStatus.Open)
            throw new Exception("It's not possible to cancel an already closed sale");
        Status = SaleStatus.Cancelled;
    }
}
