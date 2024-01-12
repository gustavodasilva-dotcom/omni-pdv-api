namespace OmniePDV.API.Data.Entities;

public sealed class Client(
    string Name,
    string SSN,
    DateTime Birthday,
    bool Active) : Entity(Guid.NewGuid())
{
    public string Name { get; private set; } = Name.Trim();
    public string SSN { get; private set; } = SSN.Trim();
    public DateTime Birthday { get; private set; } = Birthday.Date;
    public bool Active { get; private set; } = Active;

    public void SetName(string name) => Name = name.Trim();
    public void SetSSN(string ssn) => SSN = ssn.Trim();
    public void SetBirthday(DateTime birthday) => Birthday = birthday.Date;
    public void SetActive(bool active) => Active = active;
}
