using MacroDeckExtensionStoreLibrary.Enums;
using MacroDeckExtensionStoreLibrary.Exceptions;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
    public async Task<ActionResult<ExtensionSummary[]>> GetExtensionsAsync()
    {
        var extensions = await _extensionManager.GetExtensionsAsync();
        return Ok(extensions);
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<Extension>> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetByPackageIdAsync(packageId);
        if (extension == null)
        {
            throw new ErrorCodeException(StatusCodes.Status404NotFound, $"Extension with package id {packageId} not found",
                ErrorCode.PackageIdNotFound);
        }
        return Ok(extension);
    }

    [HttpGet("icon/{packageId}")]
    public async Task<ActionResult<FileStream>> GetIconAsync(string packageId)
    {
        var iconFileStream = await _extensionManager.GetIconStreamAsync(packageId);
        if (iconFileStream == null)
        {
            throw new ErrorCodeException(StatusCodes.Status404NotFound, $"Extension with package id {packageId} not found",
                ErrorCode.PackageIdNotFound);
        }

        return File(iconFileStream, "image/jpg");
    }
}