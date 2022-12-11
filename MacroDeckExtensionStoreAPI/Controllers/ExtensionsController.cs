using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtensionsController : ControllerBase
{
    private readonly ILogger<ExtensionsController> _logger;
    private readonly IExtensionsRepository _extensionsRepository;

    public ExtensionsController(ILogger<ExtensionsController> logger,
        IExtensionsRepository extensionsRepository)
    {
        _logger = logger;
        _extensionsRepository = extensionsRepository;
    }


    [HttpGet]
    public async Task<Extension[]> GetExtensionsAsync()
    {
        return await _extensionsRepository.GetExtensionsAsync();
    }

    [HttpGet("{packageId}")]
    public async Task<IActionResult> GetExtensionByPackageIdAsync(string packageId)
    {
        var extension = await _extensionsRepository.GetExtensionByPackageIdAsync(packageId);
        if (extension == null)
        {
            return NotFound("Package Id not found");
        }
        return Ok(extension);
    }

    [HttpPost]
    public async Task<IActionResult> PostExtensionAsync(Extension extension)
    {
        await _extensionsRepository.AddExtensionAsync(extension);
        return Ok(extension);
    }

    [HttpPut]
    public async Task<IActionResult> PutExtensionAsync(Extension extension)
    {
        try
        {
            await _extensionsRepository.UpdateExtensionAsync(extension);
            return Ok(extension);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Package Id not found");
        }
    }
    
}