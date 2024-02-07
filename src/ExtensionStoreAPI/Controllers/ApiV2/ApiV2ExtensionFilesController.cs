using AutoMapper;
using ExtensionStoreAPI.Core.DataTypes.ApiV2;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
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
    
    [HttpGet("{packageId}/all")]
    public async Task<ActionResult<PagedList<ApiV2ExtensionFile>>> GetExtensionFilesAsync(
        string packageId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var pagination = new Pagination(page ?? 1, pageSize ?? 20);
        var extensionFiles = await _extensionFileManager.GetFilesAsync(packageId, pagination);
        return _mapper.Map<PagedList<ApiV2ExtensionFile>>(extensionFiles)!;
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<ApiV2ExtensionFile>> GetExtensionFileAsync(
        string packageId,
        [FromQuery] string? fileVersion,
        [FromQuery] int? apiVersion = null)
    {
        var extensionFile = await _extensionFileManager.GetFileAsync(packageId, fileVersion, apiVersion);
        return _mapper.Map<ApiV2ExtensionFile>(extensionFile)!;
    }

    [HttpGet("download/{packageId}")]
    public async Task<ActionResult<byte[]>> DownloadExtensionFileAsync(
        string packageId,
        [FromQuery] string? fileVersion,
        [FromQuery] int apiVersion = 40)
    {
        var file = await _extensionFileManager.GetFileStreamAsync(packageId, fileVersion, apiVersion);
        var fileName = $"{packageId.ToLower()}_{file.Item2.ToLower()}.macroDeckExtension";
        return File(file.Item1, "application/zip", fileName);
    }
}