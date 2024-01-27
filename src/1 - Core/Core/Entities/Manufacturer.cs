namespace OmniePDV.Core.Entities;

public sealed class Manufacturer(
    string Name,
    string CRN,
    bool Active) : Entity(Guid.NewGuid())
{
    public string Name { get; private set; } = Name.Trim();
    public string CRN { get; private set; } = CRN.Trim();
    public bool Active { get; private set; } = Active;

    public void SetName(string name) => Name = name.Trim();
    public void SetCRN(string crn) => CRN = crn.Trim();
    public void SetActive(bool active) => Active = active;
}
