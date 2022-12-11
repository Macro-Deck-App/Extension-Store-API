using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreLibrary.Exceptions;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtensionsFilesController : ControllerBase
{
    private readonly ILogger<ExtensionsFilesController> _logger;
    private readonly IExtensionsRepository _extensionsRepository;
    private readonly IExtensionsFilesRepository _extensionsFilesRepository;
    private readonly IGitHubRepositoryService _gitHubRepositoryService;

    public ExtensionsFilesController(ILogger<ExtensionsFilesController> logger,
        IExtensionsRepository extensionsRepository,
        IExtensionsFilesRepository extensionsFilesRepository,
        IGitHubRepositoryService gitHubRepositoryService)
    {
        _logger = logger;
        _extensionsRepository = extensionsRepository;
        _extensionsFilesRepository = extensionsFilesRepository;
        _gitHubRepositoryService = gitHubRepositoryService;
    }
    
    [HttpGet("{packageId}")]
    public async Task<ExtensionFile[]> GetExtensionFilesAsync(string packageId)
    {
        return await _extensionsRepository.GetExtensionFilesAsync(packageId);
    }

    [HttpGet("{packageId}@{version}")]
    public async Task<IActionResult> GetExtensionFileAsync(string packageId, string version, int apiVersion = 3000)
    {
        var extensionFile = await _extensionsRepository.GetExtensionFileAsync(packageId, apiVersion, version);
        if (extensionFile == null)
        {
            return NotFound("Version not found");
        }
        return Ok(extensionFile);
    }

    [HttpGet("Download/{packageId}@{version}")]
    public async Task<IActionResult> DownloadExtensionFileAsync(string packageId, string version, int apiVersion = 3000)
    {
        var extensionFile = await _extensionsRepository.GetExtensionFileAsync(packageId, apiVersion, version);
        if (extensionFile == null)
        {
            return NotFound("Version not found");
        }

        var bytes = await _extensionsFilesRepository.GetExtensionFileBytesAsync(extensionFile);
        if (bytes == null)
        {
            return NotFound("File not found");
        }

        var fileName = $"{packageId}_{version}.macroDeckExtension";

        return File(bytes, "application/zip", fileName);
    }

    [HttpPost("Upload")]
    [ApiKey]
    public async Task<IActionResult> PostUploadExtensionFileAsync(IFormFile file)
    {
        ExtensionFile? extensionFile = null;
        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _extensionsFilesRepository.SaveExtensionFileFromStreamAsync(stream);
            var license = await _gitHubRepositoryService.GetLicenseAsync(result.ExtensionManifest.Repository);
            var descriptionHtml = await _gitHubRepositoryService.GetReadmeAsync(result.ExtensionManifest.Repository);
            extensionFile = new ExtensionFile()
            {
                Version = result.ExtensionManifest.Version,
                PackageFileName = result.PackageFileName,
                MinAPIVersion = result.ExtensionManifest.TargetPluginAPIVersion,
                IconFileName = result.IconFileName,
                UploadDateTime = DateTime.Now,
                MD5Hash = result.MD5,
                LicenseName = license.Name,
                LicenseUrl = license.Url,
                DescriptionHtml = descriptionHtml
            };
            await _extensionsRepository.AddExtensionFileAsync(result.ExtensionManifest.PackageId, extensionFile);

            return Ok(result);
        }
        catch (VersionAlreadyExistException)
        {
            if (extensionFile != null)
                await _extensionsFilesRepository.DeleteExtensionFileAsync(extensionFile);
            return Conflict("Version already exists");
        }
        catch (PackageIdNotFoundException)
        {
            if (extensionFile != null)
                await _extensionsFilesRepository.DeleteExtensionFileAsync(extensionFile);
            return NotFound("Package Id not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}