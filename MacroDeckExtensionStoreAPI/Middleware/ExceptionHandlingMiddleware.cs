using MacroDeckExtensionStoreLibrary.Enums;
using MacroDeckExtensionStoreLibrary.Exceptions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MacroDeckExtensionStoreAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger _logger = Log.ForContext<ExceptionHandlingMiddleware>();
    
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Unhandled error on request {Request}", context.Request.Path);
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var message = "Internal error: something went wrong on your request. Please contact the Macro Deck team.";
        var errorCode = ErrorCode.InternalError;
        var statusCode = StatusCodes.Status500InternalServerError;
        if (exception.GetType() == typeof(ErrorCodeException))
        {
            var errorCodeException = exception as ErrorCodeException;
            message = errorCodeException?.Message;
            errorCode = errorCodeException?.ErrorCode ?? 0;
            statusCode = errorCodeException?.StatusCode ?? StatusCodes.Status501NotImplemented;
        }
        
        object errorMessage = new
        {
            Success = false,
            Error = message,
            ErrorCode = errorCode
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(errorMessage);
    }
}