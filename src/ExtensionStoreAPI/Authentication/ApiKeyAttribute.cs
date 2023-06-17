using ExtensionStoreAPI.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExtensionStoreAPI.Authentication;

[AttributeUsage(validOn: AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderApiKeyName = "Authorization";
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderApiKeyName, out var extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        var appConfig = context.HttpContext.RequestServices.GetRequiredService<AppConfig>();
        var apiKey = $"Bearer {appConfig.ApiToken}";
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }
        await next();
    }
}