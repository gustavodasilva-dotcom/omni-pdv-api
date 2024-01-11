namespace OmniePDV.API.Data.Entities;

public sealed class Product(
    Guid UID,
    string Name,
    string Description,
    double WholesalePrice,
    double RetailPrice,
    string Barcode,
    Manufacturer Manufacturer,
    bool Active) : Entity(UID)
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    public double WholesalePrice { get; private set; } = WholesalePrice;
    public double RetailPrice { get; private set; } = RetailPrice;
    public string Barcode { get; private set; } = Barcode;
    public Manufacturer Manufacturer { get; private set; } = Manufacturer;
    public bool Active { get; private set; } = Active;

    public void SetName(string name) => Name = name;
    public void SetDescription(string description) => Description = description;
    public void SetWholesalePrice(double wholesalePrice) => WholesalePrice = wholesalePrice;
    public void SetRetailPrice(double retailPrice) => RetailPrice = retailPrice;
    public void SetBarcode(string barcode) => Barcode = barcode;
    public void SetManufacturer(Manufacturer manufacturer) => Manufacturer = manufacturer;
    public void SetActive(bool active) => Active = active;
}
