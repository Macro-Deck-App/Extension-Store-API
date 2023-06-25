using ExtensionStoreAPI.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace ExtensionStoreAPI.Core.ErrorHandling;

public class ErrorCodeException : Exception
{
    public int StatusCode { get; }
    public override string Message { get; }
    public ErrorCodes ErrorCodes { get; }

    public ErrorCodeException(ErrorCodes errorCodes)
    {
        StatusCode = errorCodes.GetStatusCode() ?? StatusCodes.Status400BadRequest;
        Message = errorCodes.GetDescription();
        ErrorCodes = errorCodes;
    }
}