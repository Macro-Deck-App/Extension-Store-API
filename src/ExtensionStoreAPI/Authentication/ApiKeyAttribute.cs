using ExtensionStoreAPI.Core.Configuration;
using ExtensionStoreAPI.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExtensionStoreAPI.Authentication;

[AttributeUsage(validOn: AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private const string AuthorizationHeader = "x-admin-token";
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeader, out var adminToken)
            || adminToken.Count > 1)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Request.Headers.Remove(AuthorizationHeader);

        if (ExtensionStoreApiConfig.AdminAuthenticationToken.EqualsCryptographically(adminToken.ToString()))
        {
            return;
        }
        
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
    }
}