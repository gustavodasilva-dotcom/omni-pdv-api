namespace OmniePDV.API.Data.Entities;

public sealed class Product(
    string Name,
    string Description,
    double WholesalePrice,
    double RetailPrice,
    string Barcode,
    Manufacturer Manufacturer,
    bool Active) : Entity(Guid.NewGuid())
{
    public string Name { get; private set; } = Name.Trim();
    public string Description { get; private set; } = Description.Trim();
    public double WholesalePrice { get; private set; } = WholesalePrice;
    public double RetailPrice { get; private set; } = RetailPrice;
    public string Barcode { get; private set; } = Barcode.Trim();
    public Manufacturer Manufacturer { get; private set; } = Manufacturer;
    public bool Active { get; private set; } = Active;

    public void SetName(string name) => Name = name.Trim();
    public void SetDescription(string description) => Description = description.Trim();
    public void SetWholesalePrice(double wholesalePrice) => WholesalePrice = wholesalePrice;
    public void SetRetailPrice(double retailPrice) => RetailPrice = retailPrice;
    public void SetBarcode(string barcode) => Barcode = barcode.Trim();
    public void SetManufacturer(Manufacturer manufacturer) => Manufacturer = manufacturer;
    public void SetActive(bool active) => Active = active;
}
