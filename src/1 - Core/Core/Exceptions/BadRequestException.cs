namespace OmniePDV.Core.Exceptions;

public sealed class BadRequestException(string message)
    : BaseException(message)
{
}
