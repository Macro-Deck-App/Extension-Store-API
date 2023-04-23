using AutoMapper;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using MacroDeckExtensionStoreLibrary.Models.ApiV2;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers.ApiV2;

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
    public async Task<ActionResult<PagedData<ApiV2ExtensionSummary[]>>> GetExtensionsAsync(
        [FromQuery] Filter filter,
        [FromQuery] Pagination pagination)
    {
        var extensionSummaries = await _extensionManager.GetExtensionsPagedAsync(filter, pagination);
        var apiV2ExtensionSummaries = _mapper.Map<PagedData<ApiV2ExtensionSummary[]>>(extensionSummaries);
        return Ok(apiV2ExtensionSummaries);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<string[]>> GetCategories([FromQuery] Filter filter)
    {
        var categories = await _extensionManager.GetCategoriesAsync(filter);
        return categories;
    }

    [HttpGet("topDownloads")]
    public async Task<ActionResult<ApiV2ExtensionSummary[]>> GetTopDownloadsOfMonth([FromQuery] Filter filter,
        int month,
        int year,
        int count = 3)
    {
        var topDownloads = await _extensionManager.GetTopDownloadsOfMonth(filter, month, year, count);
        var apiV2TopDownloads = _mapper.Map<ApiV2ExtensionSummary[]>(topDownloads);
        return Ok(apiV2TopDownloads);
    }

    [HttpGet("search/{query}")]
    public async Task<ActionResult<PagedData<ApiV2ExtensionSummary[]>>> SearchAsync(string query,
        [FromQuery] Filter filter,
        [FromQuery] Pagination pagination)
    {
        var extensionSummaries = await _extensionManager.SearchAsync(query, filter, pagination);
        var apiV2ExtensionSummaries = _mapper.Map<PagedData<ApiV2ExtensionSummary[]>>(extensionSummaries);
        return Ok(apiV2ExtensionSummaries);
    }

    [HttpGet("{packageId}")]
    public async Task<ActionResult<ApiV2Extension>> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetByPackageIdAsync(packageId);
        var apiV2Extension = _mapper.Map<ApiV2Extension>(extension);
        return Ok(apiV2Extension);
    }
    
    [HttpGet("{packageId}/summary")]
    public async Task<ActionResult<ApiV2ExtensionSummary>> GetExtensionSummaryByPackageIdAsync(string packageId)
    {
        var extension = await _extensionManager.GetSummaryByPackageIdAsync(packageId);
        var apiV2Extension = _mapper.Map<ApiV2ExtensionSummary>(extension);
        return Ok(apiV2Extension);
    }

    [HttpGet("icon/{packageId}")]
    public async Task<ActionResult<FileStream>> GetIconAsync(string packageId)
    {
        var iconFileStream = await _extensionManager.GetIconStreamAsync(packageId);
        return File(iconFileStream, "image/jpg");
    }
}