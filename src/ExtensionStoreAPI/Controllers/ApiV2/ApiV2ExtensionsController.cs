using System.Net.Mime;
using AutoMapper;
using ExtensionStoreAPI.Core.DataTypes.ApiV2;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtensionStoreAPI.Controllers.ApiV2;

/// <summary>
/// Controller for /extensions route for api version 2.0
/// DO NOT CHANGE ANY DATATYPE OR ROUTE!
/// </summary>

[Route("rest/v{version:apiVersion}/extensions")]
[ApiController]
[ApiVersion("2.0")]
public class ApiV2ExtensionsController : ControllerBase
{
    private readonly IExtensionManager _extensionManager;
    private readonly IMapper _mapper;

    public ApiV2ExtensionsController(IExtensionManager extensionManager, IMapper mapper)
    {
        _extensionManager = extensionManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ApiV2ExtensionSummary>>> GetExtensionsAsync(
        [FromQuery] string? searchString,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] Filter? filter)
    {
        var pagination = new Pagination(page ?? 1, pageSize ?? 20);
        var extensionSummaries = await _extensionManager.GetAllAsync(searchString, filter, pagination);
        return _mapper.Map<PagedList<ApiV2ExtensionSummary>>(extensionSummaries)!;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<string[]>> GetCategories([FromQuery] Filter filter)
    {
        var categories = await _extensionManager.GetCategoriesAsync(filter);
        return categories;
    }

    [HttpGet("topDownloads")]
    public async Task<ActionResult<List<ApiV2ExtensionSummary>>> GetTopDownloadsOfMonth(
        [FromQuery] Filter filter,
        int month,
        int year,
        int count = 3)
    {
        var topDownloads = await _extensionManager.GetTopDownloadsOfMonth(filter, month, year, count);
        return _mapper.Map<List<ApiV2ExtensionSummary>>(topDownloads)!;
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<ApiV2Extension>> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetByPackageIdAsync(packageId);
        return _mapper.Map<ApiV2Extension>(extension)!;
    }
    
    [HttpGet("{packageId}/summary")]
    public async Task<ActionResult<ApiV2ExtensionSummary>> GetExtensionSummaryByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetSummaryByPackageIdAsync(packageId);
        return _mapper.Map<ApiV2ExtensionSummary>(extension)!;
    }

    [HttpGet("icon/{packageId}")]
    public async Task<ActionResult<FileStream>> GetIconAsync(string packageId)
    {
        var iconFileStream = await _extensionManager.GetIconStreamAsync(packageId);
        var contentDisposition = new ContentDisposition
        {
            FileName = "icon.png",
            Inline = false
        };
        Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

        return File(iconFileStream, "image/png");
    }
}