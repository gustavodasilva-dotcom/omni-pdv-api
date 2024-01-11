using OmniePDV.API.Exceptions.Base;

namespace OmniePDV.API.Exceptions;

public sealed class NotFoundException(string message)
    : BaseException(message)
{
}
