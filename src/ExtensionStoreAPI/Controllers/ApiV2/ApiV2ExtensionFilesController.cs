using AutoMapper;
using ExtensionStoreAPI.Core.DataTypes.ApiV2;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtensionStoreAPI.Controllers.ApiV2;

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
        return _mapper.Map<ApiV2ExtensionFile[]>(extensionFiles);
    }

    [HttpGet("{packageId}@{fileVersion}")]
    public async Task<ActionResult<ApiV2ExtensionFile>> GetExtensionFileAsync(
        string packageId,
        string fileVersion,
        [FromQuery] int? apiVersion = null)
    {
        var extensionFile = await _extensionFileManager.GetFileAsync(packageId, apiVersion, fileVersion);
        return _mapper.Map<ApiV2ExtensionFile>(extensionFile);
    }

    [HttpGet("download/{packageId}@{fileVersion}")]
    public async Task<ActionResult<byte[]>> DownloadExtensionFileAsync(
        string packageId,
        string fileVersion,
        [FromQuery] int apiVersion = 3000)
    {
        var fileStream = await _extensionFileManager.GetFileStreamAsync(packageId, apiVersion, fileVersion);
        var fileName = $"{packageId.ToLower()}_{fileVersion.ToLower()}.macroDeckExtension";
        return File(fileStream, "application/zip", fileName);
    }
}