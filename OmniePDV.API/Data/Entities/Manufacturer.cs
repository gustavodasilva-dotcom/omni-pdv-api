namespace OmniePDV.API.Data.Entities;

public sealed class Manufacturer(Guid UID, string Name, string CRN, bool Active)
    : Entity(UID)
{
    public string Name { get; private set; } = Name;
    public string CRN { get; private set; } = CRN;
    public bool Active { get; private set; } = Active;

    public void SetName(string name) => Name = name;
    public void SetCRN(string crn) => CRN = crn;
    public void SetActive(bool active) => Active = active;
}
