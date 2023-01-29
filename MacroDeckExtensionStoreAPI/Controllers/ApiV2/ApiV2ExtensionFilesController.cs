using AutoMapper;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models.ApiV2;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers.ApiV2;

/// <summary>
/// Controller for /files route for api version 2.0
/// DO NOT CHANGE ANY DATATYPE OR ROUTE!
/// </summary>

[Route("rest/v{version:apiVersion}/files")]
[ApiController]
[ApiVersion("2.0")]
public class ApiV2ExtensionFilesController : ControllerBase
{
    private readonly IExtensionFileManager _extensionFileManager;
    private readonly IMapper _mapper;

    public ApiV2ExtensionFilesController(IExtensionFileManager extensionFileManager, IMapper mapper)
    {
        _extensionFileManager = extensionFileManager;
        _mapper = mapper;
    }
    
    [HttpGet("{packageId}")]
    public async Task<ActionResult<ApiV2ExtensionFile[]>> GetExtensionFilesAsync(string packageId)
    {
        var extensionFiles = await _extensionFileManager.GetFilesAsync(packageId);
        var apiV2ExtensionFiles = _mapper.Map<ApiV2ExtensionFile[]>(extensionFiles);
        return Ok(apiV2ExtensionFiles);
    }

    [HttpGet("{packageId}@{fileVersion}")]
    public async Task<ActionResult<ApiV2ExtensionFile>> GetExtensionFileAsync(string packageId, string fileVersion, int? apiVersion = null)
    {
        var extensionFile = await _extensionFileManager.GetFileAsync(packageId, apiVersion, fileVersion);
        var apiV2ExtensionFile = _mapper.Map<ApiV2ExtensionFile>(extensionFile);
        return Ok(apiV2ExtensionFile);
    }

    [HttpGet("Download/{packageId}@{fileVersion}")]
    public async Task<ActionResult<byte[]>> DownloadExtensionFileAsync(string packageId, string fileVersion, int apiVersion = 3000)
    {
        var fileBytes = await _extensionFileManager.GetFileBytesAsync(packageId, apiVersion, fileVersion);
        var fileName = $"{packageId.ToLower()}_{fileVersion.ToLower()}.macroDeckExtension";
        return File(fileBytes, "application/zip", fileName);
    }
}