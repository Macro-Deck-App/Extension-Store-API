using ExtensionStoreAPI.Authentication;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtensionStoreAPI.Controllers;

[Route("rest/files")]
[ApiController]
public class ExtensionFilesController : Controller
{
    private readonly IExtensionFileManager _extensionFileManager;

    public ExtensionFilesController(IExtensionFileManager extensionFileManager)
    {
        _extensionFileManager = extensionFileManager;
    }
    
    [HttpPost("upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
    [RequestSizeLimit(209715200)]
    [ApiKey]
    public async Task<ActionResult<ExtensionFileUploadResult>> PostUploadExtensionFileAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var result = await _extensionFileManager.CreateFileAsync(stream);
        return Created("", result);
    }
}