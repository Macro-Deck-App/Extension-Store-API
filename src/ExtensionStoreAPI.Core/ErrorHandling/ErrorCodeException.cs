using ExtensionStoreAPI.Core.Extensions;

namespace ExtensionStoreAPI.Core.ErrorHandling;

public class ErrorCodeException : Exception
{
    public int StatusCode { get; }
    public override string Message { get; }
    public ErrorCodes ErrorCodes { get; }

    public ErrorCodeException(ErrorCodes errorCodes)
    {
        StatusCode = errorCodes.GetStatusCode() ?? 400;
        Message = errorCodes.GetDescription();
        ErrorCodes = errorCodes;
    }
}