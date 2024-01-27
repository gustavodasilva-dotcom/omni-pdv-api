namespace OmniePDV.Core.Abstractions.Models;

public abstract class BaseInputModel
{
    public virtual bool ValidateModel() =>
        throw new NotImplementedException();
}
