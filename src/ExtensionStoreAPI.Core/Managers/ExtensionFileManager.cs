using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.GitHub;
using ExtensionStoreAPI.Core.DataTypes.MacroDeck;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.Exceptions;
using ExtensionStoreAPI.Core.Interfaces;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using ExtensionStoreAPI.Core.Utils;
using JetBrains.Annotations;
using Serilog;

namespace ExtensionStoreAPI.Core.Managers;

[UsedImplicitly]
public class ExtensionFileManager : IExtensionFileManager
{
    private readonly ILogger _logger = Log.ForContext<ExtensionFileManager>();
    private readonly IExtensionFileRepository _extensionFileRepository;
    private readonly IExtensionManager _extensionManager;
    private readonly IGitHubRepositoryService _gitHubRepositoryService;
    private readonly IExtensionRepository _extensionRepository;
    private readonly IExtensionDownloadInfoRepository _extensionDownloadInfoRepository;
    private readonly IMapper _mapper;

    public ExtensionFileManager(IExtensionFileRepository extensionFileRepository,
        IExtensionManager extensionManager,
        IGitHubRepositoryService gitHubRepositoryService,
        IExtensionRepository extensionRepository,
        IExtensionDownloadInfoRepository extensionDownloadInfoRepository,
        IMapper mapper)
    {
        _extensionFileRepository = extensionFileRepository;
        _extensionManager = extensionManager;
        _gitHubRepositoryService = gitHubRepositoryService;
        _extensionRepository = extensionRepository;
        _extensionDownloadInfoRepository = extensionDownloadInfoRepository;
        _mapper = mapper;
    }
    
    public async Task<bool> ExistAsync(string packageId, string version)
    {
        var exist = await _extensionFileRepository.ExistAsync(packageId, version);
        return exist;
    }

    public async Task<ExtensionFile[]> GetFilesAsync(string packageId)
    {
        var extensionFileEntities = await _extensionFileRepository.GetFilesAsync(packageId);
        if (extensionFileEntities.Length == 0)
        {
            return Array.Empty<ExtensionFile>();
        }
        var extensionFiles = _mapper.Map<ExtensionFile[]>(extensionFileEntities);
        return extensionFiles;
    }

    public async Task<ExtensionFile> GetFileAsync(string packageId, int? targetApiVersion = null, string version = "latest")
    {
        var extensionFileEntity = await _extensionFileRepository.GetFileAsync(packageId, targetApiVersion, version);
        if (extensionFileEntity == null)
        {
            ErrorCodeExceptions.VersionNotFoundException();
        }
        var extensionFile = _mapper.Map<ExtensionFile>(extensionFileEntity);
        return extensionFile;
    }

    public async Task<ExtensionFileUploadResult> CreateFileAsync(Stream packageStream)
    {
        var tmpFilePath = await CreateTempFile(packageStream);

        if (!File.Exists(tmpFilePath))
        {
            _logger.Fatal("Cannot save package file from stream");
            throw ErrorCodeExceptions.InternalErrorException();
        }
        
        var extensionManifest = await GetExtensionManifest(tmpFilePath);

        var extensionExists = await _extensionManager.ExistsAsync(extensionManifest.PackageId!);
        if (!extensionExists)
        {
            _logger.Information("{PackageId} does not exist yet", extensionManifest.PackageId);
            var extension = _mapper.Map<Extension>(extensionManifest);
            await _extensionManager.CreateAsync(extension);
        }
        
        var versionExists =
            await _extensionFileRepository.ExistAsync(extensionManifest.PackageId!, extensionManifest.Version!);
        if (versionExists)
        {
            throw ErrorCodeExceptions.VersionAlreadyExistsException();
        }
        
        var finalFileName = GenerateUniqueFileName(extensionManifest);
        var packageFileName = $"{finalFileName}.zip";
        var iconFileName = $"{finalFileName}.png";
        
        var finalIconPath = await SaveIcon(extensionManifest, tmpFilePath, iconFileName);

        var finalPackageFilePath = Path.Combine(Paths.DataDirectory, packageFileName);

        try
        {
            File.Copy(tmpFilePath, finalPackageFilePath);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Cannot copy final package {PackageId}", extensionManifest.PackageId);
            throw;
        }

        var fileHash = await Sha256Utils.CalculateSha256Hash(finalPackageFilePath);
        var readmeHtml = "";
        var description = "";
        var license = new GitHubLicense();

        try
        {
            readmeHtml = await _gitHubRepositoryService.GetReadmeAsync(extensionManifest.Repository);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Cannot get readme of {PackageId}", extensionManifest.PackageId);
        }
        try
        {
            description = await _gitHubRepositoryService.GetDescriptionAsync(extensionManifest.Repository);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Cannot get description of {PackageId}", extensionManifest.PackageId);
        }
        try
        {
            license = await _gitHubRepositoryService.GetLicenseAsync(extensionManifest.Repository);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Cannot get license of {PackageId}", extensionManifest.PackageId);
        }

        var currentFile =
            await _extensionFileRepository.GetFileAsync(extensionManifest.PackageId!,
                extensionManifest.TargetPluginApiVersion);

        var result = new ExtensionFileUploadResult
        {
            Success = true,
            NewPlugin = !extensionExists,
            ExtensionManifest = extensionManifest,
            PackageFileName = packageFileName,
            IconFileName = iconFileName,
            FileHash = fileHash,
            LicenseName = license.Name,
            LicenseUrl = license.Url,
            ReadmeHtml = readmeHtml,
            Description = description,
            NewVersion = extensionManifest.Version,
            CurrentVersion = currentFile?.Version
        };

        var extensionFileEntity = _mapper.Map<ExtensionFileEntity>(result);

        try
        {
            await _extensionFileRepository.CreateFileAsync(extensionManifest.PackageId!, extensionFileEntity);
        }
        catch
        {
            SafeDelete.Delete(finalPackageFilePath);
            SafeDelete.Delete(finalIconPath);

            throw;
        }
        finally
        {
            SafeDelete.Delete(tmpFilePath);
        }
        
        await _extensionRepository.UpdateDescription(extensionManifest.PackageId!, description);
        return result;
    }

    private async Task<string> SaveIcon(ExtensionManifest extensionManifest, string tmpFilePath, string iconFileName)
    {
        Stream? iconMemoryStream;
        if (extensionManifest.Type == ExtensionType.Plugin)
        {
            iconMemoryStream = await ExtensionIconExtractor.FromZipFilePathAsync(tmpFilePath);
        }
        else
        {
            iconMemoryStream = await ExtensionIconExtractor.GeneratePreviewImage(tmpFilePath);
        }

        if (iconMemoryStream == null)
        {
            _logger.Fatal("Failed to extract icon");
            throw ErrorCodeExceptions.InternalErrorException();
        }

        var finalIconPath = Path.Combine(Paths.DataDirectory, iconFileName);
        await using var iconFileStream = File.Create(finalIconPath);
        iconMemoryStream.Seek(0, SeekOrigin.Begin);
        await iconMemoryStream.CopyToAsync(iconFileStream);
        iconMemoryStream.Close();
        iconFileStream.Close();
        
        return finalIconPath;
    }

    private static async Task<ExtensionManifest> GetExtensionManifest(string path)
    {
        var extensionManifest = await ExtensionManifest.FromZipFilePathAsync(path);
        if (extensionManifest == null)
        {
            throw ErrorCodeExceptions.ExtensionManifestNotFoundException();
        }

        if (string.IsNullOrWhiteSpace(extensionManifest.PackageId)
            || string.IsNullOrWhiteSpace(extensionManifest.Version))
        {
            throw ErrorCodeExceptions.ExtensionManifestNotFoundException();
        }

        return extensionManifest;
    }

    private static async Task<string> CreateTempFile(Stream packageStream)
    {
        var tmpFilePath = Path.Combine(Paths.TempDirectory, Guid.NewGuid().ToString());

        await using var tmpFileStream = File.Create(tmpFilePath);
        packageStream.Seek(0, SeekOrigin.Begin);
        await packageStream.CopyToAsync(tmpFileStream);
        tmpFileStream.Close();
        packageStream.Close();
        await tmpFileStream.DisposeAsync();
        await packageStream.DisposeAsync();
        return tmpFilePath;
    }

    public async Task<FileStream> GetFileStreamAsync(string packageId, int targetApiVersion, string version)
    {
        var extensionFile = await _extensionFileRepository.GetFileAsync(packageId, targetApiVersion, version);
        if (extensionFile == null)
        {
            throw ErrorCodeExceptions.VersionNotFoundException();
        }
        var filePath = Path.Combine(Paths.DataDirectory, extensionFile.PackageFileName);
        if (!File.Exists(filePath))
        {
            _logger.Fatal("Package for {PackageId} does not exist", packageId);
            throw ErrorCodeExceptions.FileNotFoundException();
        }

        try
        {
            await _extensionDownloadInfoRepository.IncreaseDownloadCounter(packageId, extensionFile.Version);
            return File.Open(filePath, FileMode.Open);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error while reading package file for {PackageId}", packageId);
            throw ErrorCodeExceptions.InternalErrorException();
        }
    }
    
    private static string GenerateUniqueFileName(ExtensionManifest extensionManifest)
    {
        var guid = Guid.NewGuid().ToString();
        var fileName = $"{extensionManifest.PackageId}_{extensionManifest.Version}_{guid}";
        return fileName;
    }
}