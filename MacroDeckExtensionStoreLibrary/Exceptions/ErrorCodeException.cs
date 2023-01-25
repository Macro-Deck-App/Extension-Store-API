using MacroDeckExtensionStoreLibrary.Enums;

namespace MacroDeckExtensionStoreLibrary.Exceptions;

public class ErrorCodeException : Exception
{
    public int StatusCode { get; }
    public override string Message { get; }
    public ErrorCode ErrorCode { get; }

    public ErrorCodeException(int statusCode, string message, ErrorCode errorCode)
    {
        StatusCode = statusCode;
        Message = message;
        ErrorCode = errorCode;
    }
}