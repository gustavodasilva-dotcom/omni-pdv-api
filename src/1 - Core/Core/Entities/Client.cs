namespace OmniePDV.Core.Entities;

public sealed class Client(
    string name,
    string ssn,
    DateTime birthday,
    string? email,
    bool active) : Entity(Guid.NewGuid())
{
    public string Name { get; private set; } = name.Trim();
    public string SSN { get; private set; } = ssn.Trim();
    public DateTime Birthday { get; private set; } = birthday.Date;
    public string? Email { get; private set; } = email?.Trim();
    public bool Active { get; private set; } = active;

    public void SetName(string name) => Name = name.Trim();
    public void SetSSN(string ssn) => SSN = ssn.Trim();
    public void SetBirthday(DateTime birthday) => Birthday = birthday.Date;
    public void SetEmail(string? email) => Email = email?.Trim();
    public void SetActive(bool active) => Active = active;
}
