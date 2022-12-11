using MacroDeckExtensionStoreAPI.Authentication;
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
    private readonly IExtensionsFilesRepository _extensionsFilesRepository;

    public ExtensionsController(ILogger<ExtensionsController> logger,
        IExtensionsRepository extensionsRepository,
        IExtensionsFilesRepository extensionsFilesRepository)
    {
        _logger = logger;
        _extensionsRepository = extensionsRepository;
        _extensionsFilesRepository = extensionsFilesRepository;
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

    [HttpGet("Icon/{packageId}")]
    public async Task<IActionResult> GetIconAsync(string packageId)
    {
        var extensionFiles = await _extensionsRepository.GetExtensionFilesAsync(packageId);
        if (extensionFiles.Length == 0)
        {
            return NotFound("Package Id does not exist or does not contain any files yet.");
        }
        var latestExtensionFile = extensionFiles.OrderBy(x => x.UploadDateTime).First();
        var iconPath = Path.Combine(_extensionsFilesRepository.ExtensionsPath, latestExtensionFile.IconFileName);
        if (!System.IO.File.Exists(iconPath))
        {
            return NotFound("No icon file found");
        }

        var iconFileStream = System.IO.File.Open(iconPath, FileMode.Open, FileAccess.Read, FileShare.None);
        iconFileStream.Seek(0, SeekOrigin.Begin);

        return File(iconFileStream, "image/jpg");
    }
    

    [HttpPost]
    [ApiKey]
    public async Task<IActionResult> PostExtensionAsync(Extension extension)
    {
        await _extensionsRepository.AddExtensionAsync(extension);
        return Ok(extension);
    }

    [HttpPut]
    [ApiKey]
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