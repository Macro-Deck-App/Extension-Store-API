using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MacroDeckExtensionStoreAPI.Controllers;

[ApiController]
[Route("extensions/files")]
public class ExtensionsFilesController : ControllerBase
{
    private readonly IExtensionFileManager _extensionFileManager;
    private readonly IExtensionManager _extensionManager;
    private readonly ILogger _logger = Log.ForContext<ExtensionsFilesController>();

    public ExtensionsFilesController(IExtensionFileManager extensionFileManager, IExtensionManager extensionManager)
    {
        _extensionFileManager = extensionFileManager;
        _extensionManager = extensionManager;
    }
    
    [HttpGet("{packageId}")]
    public async Task<ActionResult<ExtensionFile[]>> GetExtensionFilesAsync(string packageId)
    {
        var extensionFiles = await _extensionFileManager.GetFilesAsync(packageId);
        if (extensionFiles == null)
        {
            return NotFound("PackageId not found");
        }

        return Ok(extensionFiles);
    }

    [HttpGet("{packageId}@{version}")]
    public async Task<ActionResult<ExtensionFile>> GetExtensionFileAsync(string packageId, string version, int? apiVersion = null)
    {
        var extensionFile = await _extensionFileManager.GetFileAsync(packageId, apiVersion, version);
        if (extensionFile == null)
        {
            return NotFound("PackageId or version not found");
        }
        return Ok(extensionFile);
    }

    [HttpGet("Download/{packageId}@{version}")]
    public async Task<ActionResult<byte[]>> DownloadExtensionFileAsync(string packageId, string version, int apiVersion = 3000)
    {
        var fileBytes = await _extensionFileManager.GetFileBytesAsync(packageId, version);
        if (fileBytes == null)
        {
            return NotFound("File not found");
        }

        await _extensionManager.CountDownloadAsync(packageId, version);

        var fileName = $"{packageId.ToLower()}_{version.ToLower()}.macroDeckExtension";
        
        return File(fileBytes, "application/zip", fileName);
    }

    [HttpPost("Upload")]
    [ApiKey]
    public async Task<ActionResult<ExtensionFile>> PostUploadExtensionFileAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var extensionFile = await _extensionFileManager.CreateFileAsync(stream);
        if (extensionFile == null)
        {
            return StatusCode(500);
        }

        return Created("",extensionFile);
        /*
        try
        {
            var result = await _extensionsFilesRepository.SaveExtensionFileFromStreamAsync(stream);
            var license = await _gitHubRepositoryService.GetLicenseAsync(result.ExtensionManifest.Repository);
            var descriptionHtml = await _gitHubRepositoryService.GetReadmeAsync(result.ExtensionManifest.Repository);
            extensionFile = _mapper.Map<ExtensionFile>(result);
            extensionFile.LicenseName = license.Name;
            extensionFile.LicenseUrl = license.Url;
            extensionFile.DescriptionHtml = descriptionHtml;
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
        */
    }
}