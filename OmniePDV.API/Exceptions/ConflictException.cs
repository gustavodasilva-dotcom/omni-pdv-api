using OmniePDV.API.Exceptions.Base;

namespace OmniePDV.API.Exceptions;

public sealed class ConflictException(string message)
    : BaseException(message)
{
}
