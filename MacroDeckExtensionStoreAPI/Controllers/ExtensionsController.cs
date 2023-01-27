using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("extensions")]
public class ExtensionsController : ControllerBase
{
    private readonly IExtensionManager _extensionManager;
    
    public ExtensionsController(IExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    [HttpGet]
    public async Task<ActionResult<PagedData<ExtensionSummary[]>>> GetExtensionsAsync(
        [FromQuery] Filter filter,
        [FromQuery] Pagination pagination)
    {
        var extensions = await _extensionManager.GetExtensionsPagedAsync(filter, pagination);
        return Ok(extensions);
    }

    [HttpGet("topDownloads")]
    public async Task<ActionResult<ExtensionSummary[]>> GetTopDownloadsOfMonth([FromQuery] Filter filter, int month, int year, int count = 3)
    {
        var topDownloads = await _extensionManager.GetTopDownloadsOfMonth(filter, month, year, count);
        return Ok(topDownloads);
    }

    [HttpGet("search/{query}")]
    public async Task<ActionResult<ExtensionSummary[]>> SearchAsync(string query)
    {
        var extensions = await _extensionManager.SearchAsync(query);
        return Ok(extensions);
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<Extension>> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetByPackageIdAsync(packageId);
        return Ok(extension);
    }

    [HttpGet("icon/{packageId}")]
    public async Task<ActionResult<FileStream>> GetIconAsync(string packageId)
    {
        var iconFileStream = await _extensionManager.GetIconStreamAsync(packageId);
        return File(iconFileStream, "image/jpg");
    }
}