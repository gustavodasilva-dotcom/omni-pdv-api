namespace OmniePDV.Core.Exceptions;

public sealed class NotFoundException(string message)
    : BaseException(message)
{
}
