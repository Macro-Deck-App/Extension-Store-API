using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MacroDeckExtensionStoreAPI.Authentication;

[AttributeUsage(validOn: AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string _appSettingsApiKeyName = "ApiKey";
    private const string _headerApiKeyName = "Authorization";
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(_headerApiKeyName, out var extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401
            };
            return;
        }
        var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = $"Bearer {appSettings.GetValue<string>(_appSettingsApiKeyName)}";
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403
            };
            return;
        }
        await next();
    }
}