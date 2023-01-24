using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("extensions")]
public class ExtensionsController : ControllerBase
{
    private readonly IExtensionManager _extensionManager;
    private readonly ILogger _logger = Log.ForContext<ExtensionsController>();
    
    public ExtensionsController(IExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }


    [HttpGet]
    public async Task<ActionResult<ExtensionSummary[]>> GetExtensionsAsync()
    {
        try
        {
            var extensions = await _extensionManager.GetExtensionsAsync();
            return Ok(extensions);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Cannot return extensions");
            return NoContent();
        }
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<Extension>> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetByPackageIdAsync(packageId);
        if (extension == null)
        {
            return NotFound("Package Id not found");
        }
        return Ok(extension);
    }

    [HttpGet("icon/{packageId}")]
    public async Task<ActionResult<FileStream>> GetIconAsync(string packageId)
    {
        await using var iconFileStream = await _extensionManager.GetIconStreamAsync(packageId);
        if (iconFileStream == null)
        {
            return NotFound("Icon not found");
        }

        return File(iconFileStream, "image/jpg");
    }
    
    [HttpPost]
    [ApiKey]
    public async Task<IActionResult> PostExtensionAsync(Extension extension)
    {
        await _extensionManager.CreateAsync(extension);
        return Ok(extension);
    }

}