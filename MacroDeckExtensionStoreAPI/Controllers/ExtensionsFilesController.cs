using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("extensions/files")]
public class ExtensionsFilesController : ControllerBase
{
    private readonly IExtensionFileManager _extensionFileManager;

    public ExtensionsFilesController(IExtensionFileManager extensionFileManager)
    {
        _extensionFileManager = extensionFileManager;
    }
    
    [HttpGet("{packageId}")]
    public async Task<ActionResult<ExtensionFile[]>> GetExtensionFilesAsync(string packageId)
    {
        var extensionFiles = await _extensionFileManager.GetFilesAsync(packageId);
        if (extensionFiles == null)
        {
            return NotFound("PackageId not found");
        }
        return Ok(extensionFiles);
    }

    [HttpGet("{packageId}@{version}")]
    public async Task<ActionResult<ExtensionFile>> GetExtensionFileAsync(string packageId, string version, int? apiVersion = null)
    {
        var extensionFile = await _extensionFileManager.GetFileAsync(packageId, apiVersion, version);
        if (extensionFile == null)
        {
            return NotFound("PackageId or version not found");
        }
        return Ok(extensionFile);
    }

    [HttpGet("Download/{packageId}@{version}")]
    public async Task<ActionResult<byte[]>> DownloadExtensionFileAsync(string packageId, string version, int apiVersion = 3000)
    {
        var fileBytes = await _extensionFileManager.GetFileBytesAsync(packageId, apiVersion, version);
        if (fileBytes == null)
        {
            return NotFound("File not found");
        }
        var fileName = $"{packageId.ToLower()}_{version.ToLower()}.macroDeckExtension";
        return File(fileBytes, "application/zip", fileName);
    }

    [HttpPost("Upload")]
    [ApiKey]
    public async Task<ActionResult<ExtensionFileUploadResult>> PostUploadExtensionFileAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var result = await _extensionFileManager.CreateFileAsync(stream);
        if (result == null)
        {
            throw new Exception();
        }
        return Created(result.ExtensionManifest!.PackageId, result);
    }
}