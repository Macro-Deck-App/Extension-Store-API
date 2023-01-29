using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers;

[Route("rest/files")]
[ApiController]
public class ExtensionFilesController : Controller
{
    private readonly IExtensionFileManager _extensionFileManager;

    public ExtensionFilesController(IExtensionFileManager extensionFileManager)
    {
        _extensionFileManager = extensionFileManager;
    }
    
    [HttpPost("Upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
    [RequestSizeLimit(209715200)]
    [ApiKey]
    public async Task<ActionResult<ExtensionFileUploadResult>> PostUploadExtensionFileAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var result = await _extensionFileManager.CreateFileAsync(stream);
        return Created(result.ExtensionManifest!.PackageId, result);
    }
}