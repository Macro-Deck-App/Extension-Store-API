using ExtensionStoreAPI.Authentication;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtensionStoreAPI.Controllers;

[Route("rest/system")]
[ApiController]
public class SystemController : Controller
{
    private readonly IExtensionManager _extensionManager;

    public SystemController(IExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    [HttpDelete("clear/{confirm}")]
    [ApiKey]
    public async Task<IActionResult> ClearDatabase(bool confirm)
    {
        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
        if (!string.IsNullOrWhiteSpace(environment) && environment != "DEVELOPMENT")
        {
            return NotFound();
        }
        if (!confirm)
        {
            return BadRequest();
        }
        await _extensionManager.DeleteAllAsync();
        return Ok();
    }
}