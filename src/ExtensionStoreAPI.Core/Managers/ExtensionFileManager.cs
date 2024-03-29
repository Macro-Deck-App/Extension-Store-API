using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.GitHub;
using ExtensionStoreAPI.Core.DataTypes.MacroDeck;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.Enums;
using ExtensionStoreAPI.Core.ErrorHandling;
using ExtensionStoreAPI.Core.Interfaces;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using ExtensionStoreAPI.Core.Utils;
using JetBrains.Annotations;
using Serilog;
using ILogger = Serilog.ILogger;

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

    public async Task<PagedList<ExtensionFile>> GetFilesAsync(string packageId, Pagination pagination)
    {
        var extensionFileEntities = await _extensionFileRepository.GetFilesAsync(packageId, pagination);
        return _mapper.Map<PagedList<ExtensionFile>>(extensionFileEntities)!;
    }

    public async Task<ExtensionFile> GetFileAsync(string packageId, string? version, int? targetApiVersion = null)
    {
        var extensionFileEntity = await _extensionFileRepository.GetFileAsync(packageId, version, targetApiVersion);
        if (extensionFileEntity == null)
        {
            throw new ErrorCodeException(ErrorCodes.VersionNotFound);
        }
        var extensionFile = _mapper.Map<ExtensionFile>(extensionFileEntity)!;
        return extensionFile;
    }

    public async Task<ExtensionFileUploadResult> CreateFileAsync(Stream packageStream)
    {
        var tmpFilePath = await CreateTempFile(packageStream);

        if (!File.Exists(tmpFilePath))
        {
            _logger.Fatal("Cannot save package file from stream");
            throw new ErrorCodeException(ErrorCodes.InternalError);
        }
        
        var extensionManifest = await GetExtensionManifest(tmpFilePath);

        var extensionExists = await _extensionManager.ExistsAsync(extensionManifest.PackageId!);
        if (!extensionExists)
        {
            _logger.Information("{PackageId} does not exist yet", extensionManifest.PackageId);
            var extension = _mapper.Map<Extension>(extensionManifest)!;
            await _extensionManager.CreateAsync(extension);
        }
        
        var versionExists =
            await _extensionFileRepository.ExistAsync(extensionManifest.PackageId!, extensionManifest.Version!);
        if (versionExists)
        {
            throw new ErrorCodeException(ErrorCodes.VersionAlreadyExists);
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
        var readme = "";
        var description = "";
        var license = new GitHubLicense();

        try
        {
            readme = await _gitHubRepositoryService.GetReadmeAsync(extensionManifest.Repository);
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

        var currentFile = await _extensionFileRepository.GetFileAsync(
                extensionManifest.PackageId!,
                targetApiVersion: extensionManifest.TargetPluginApiVersion);

        var result = new ExtensionFileUploadResult
        {
            PackageId = extensionManifest.PackageId!,
            Name = extensionManifest.Name!,
            Author = extensionManifest.Author!,
            Repository = extensionManifest.Repository!,
            MinApiVersion = extensionManifest.TargetPluginApiVersion!.Value,
            Success = true,
            NewPlugin = !extensionExists,
            PackageFileName = packageFileName,
            IconFileName = iconFileName,
            FileHash = fileHash,
            LicenseName = license.Name,
            LicenseUrl = license.Url,
            Description = description,
            NewVersion = extensionManifest.Version!,
            CurrentVersion = currentFile?.Version ?? string.Empty
        };

        var extensionFileEntity = new ExtensionFileEntity
        {
            Version = extensionManifest.Version!,
            MinApiVersion = extensionManifest.TargetPluginApiVersion!.Value,
            PackageFileName = packageFileName,
            IconFileName = iconFileName,
            FileHash = fileHash,
            LicenseName = license.Name,
            LicenseUrl = license.Url,
            Readme = readme
        };

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
        await _extensionRepository.UpdateAuthorDiscordUserId(extensionManifest.PackageId!,
            extensionManifest.AuthorDiscordUserId);
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
            throw new ErrorCodeException(ErrorCodes.InternalError);
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
            throw new ErrorCodeException(ErrorCodes.ExtensionManifestNotFound);
        }

        if (string.IsNullOrWhiteSpace(extensionManifest.PackageId)
            || string.IsNullOrWhiteSpace(extensionManifest.Version)
            || string.IsNullOrWhiteSpace(extensionManifest.Author)
            || string.IsNullOrWhiteSpace(extensionManifest.Name)
            || string.IsNullOrWhiteSpace(extensionManifest.Repository)
            || !extensionManifest.TargetPluginApiVersion.HasValue)
        {
            throw new ErrorCodeException(ErrorCodes.ExtensionManifestInvalid);
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
        await packageStream.DisposeAsync();
        return tmpFilePath;
    }

    public async Task<Tuple<FileStream, string>> GetFileStreamAsync(string packageId, string? version, int targetApiVersion)
    {
        var extensionFile = await _extensionFileRepository.GetFileAsync(packageId, version, targetApiVersion);
        if (extensionFile == null)
        {
            throw new ErrorCodeException(ErrorCodes.VersionNotFound);
        }
        var filePath = Path.Combine(Paths.DataDirectory, extensionFile.PackageFileName);
        if (!File.Exists(filePath))
        {
            _logger.Fatal("Package for {PackageId} does not exist", packageId);
            throw new ErrorCodeException(ErrorCodes.FileNotFound);
        }

        try
        {
            await _extensionDownloadInfoRepository.IncreaseDownloadCounter(packageId, extensionFile.Version);
            return Tuple.Create(File.Open(filePath, FileMode.Open), extensionFile.Version);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error while reading package file for {PackageId}", packageId);
            throw new ErrorCodeException(ErrorCodes.InternalError);
        }
    }
    
    private static string GenerateUniqueFileName(ExtensionManifest extensionManifest)
    {
        var guid = Guid.NewGuid().ToString();
        var fileName = $"{extensionManifest.PackageId}_{extensionManifest.Version}_{guid}";
        return fileName;
    }
}