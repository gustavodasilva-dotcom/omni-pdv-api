using OmniePDV.Core.Enumerations;

namespace OmniePDV.Core.Entities;

public sealed class Discount
{
    public Discount(DiscountTypeEnum Type, double Value)
    {
        this.Type = Type;
        this.Value = Value;
    }

    private Discount()
    {        
    }

    public DiscountTypeEnum Type { get; private set; }
    public double Value { get; private set; }

    public void SetType(DiscountTypeEnum Type) => this.Type = Type;
    public double SetValue(double Value) => this.Value = Value;
}
