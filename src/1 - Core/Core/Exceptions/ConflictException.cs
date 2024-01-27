namespace OmniePDV.Core.Exceptions;

public sealed class ConflictException(string message)
    : BaseException(message)
{
}
