using OmniePDV.API.Exceptions.Base;

namespace OmniePDV.API.Exceptions;

public sealed class BadRequestException(string message)
    : BaseException(message)
{
}
