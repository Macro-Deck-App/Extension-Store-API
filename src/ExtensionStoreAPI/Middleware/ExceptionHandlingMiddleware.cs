using ExtensionStoreAPI.Core.Exceptions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ExtensionStoreAPI.Middleware;

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
        if (exception.GetType() != typeof(ErrorCodeException))
        {
            exception = ErrorCodeExceptions.InternalErrorException();
        }
        
        var errorCodeException = exception as ErrorCodeException;
        var message = errorCodeException?.Message;
        var errorCode = errorCodeException?.ErrorCode ?? 0;
        var statusCode = errorCodeException?.StatusCode ?? StatusCodes.Status501NotImplemented;
        
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