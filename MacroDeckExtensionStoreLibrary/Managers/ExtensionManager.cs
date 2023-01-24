using AutoMapper;
using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Serilog;

namespace MacroDeckExtensionStoreLibrary.Managers;

[UsedImplicitly]
public class ExtensionManager : IExtensionManager
{
    private readonly ILogger _logger = Log.ForContext<ExtensionManager>();
    private readonly IExtensionRepository _extensionRepository;
    private readonly IExtensionFileRepository _extensionFileRepository;
    private readonly IMapper _mapper;

    public ExtensionManager(IExtensionRepository extensionRepository,
        IExtensionFileRepository extensionFileRepository,
        IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _extensionFileRepository = extensionFileRepository;
        _mapper = mapper;
    }

    public async Task<ExtensionSummary[]> GetExtensionsAsync()
    {
        var extensionEntities = await _extensionRepository.GetExtensionsAsync();
        if (extensionEntities.Length == 0)
        {
            return Array.Empty<ExtensionSummary>();
        }
        var extensions = _mapper.Map<ExtensionSummary[]>(extensionEntities);
        return extensions;
    }

    public async Task<Extension?> GetByPackageIdAsync(string packageId)
    {
        var extensionEntity = await _extensionRepository.GetByPackageIdAsync(packageId);
        if (extensionEntity == null)
        {
            return null;
        }

        var extension = _mapper.Map<Extension>(extensionEntity);
        return extension;
    }

    public async Task<bool> ExistsAsync(string packageId)
    {
        var exists = await _extensionRepository.ExistAsync(packageId);
        return exists;
    }

    public async Task<ExtensionSummary[]> SearchAsync(string query)
    {
        if (query.Length < 3)
        {
            return Array.Empty<ExtensionSummary>();
        }
        var extensionEntities = await _extensionRepository.SearchAsync(query);
        if (extensionEntities.Length == 0)
        {
            return Array.Empty<ExtensionSummary>();
        }
        var extensions = _mapper.Map<ExtensionSummary[]>(extensionEntities);
        return extensions;
    }

    public async Task CreateAsync(Extension extension)
    {
        try
        {
            var extensionEntity = _mapper.Map<ExtensionEntity>(extension);
            await _extensionRepository.CreateExtensionAsync(extensionEntity);
            _logger.Information("Created extension {PackageId}", extension.PackageId);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Failed to create extension {PackageId}", extension.PackageId);
        }
    }

    public async Task DeleteAsync(string packageId)
    {
        try
        {
            await _extensionRepository.DeleteExtensionAsync(packageId);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Failed to delete extension {PackageId}", packageId);
        }
    }

    public async Task<FileStream?> GetIconStreamAsync(string packageId)
    {
        var extensionFile = await _extensionFileRepository.GetFileAsync(packageId);
        if (extensionFile == null)
        {
            return null;
        }
        var iconPath = Path.Combine(Paths.DataDirectory, extensionFile.IconFileName);
        if (!File.Exists(iconPath))
        {
            _logger.Fatal("Icon file for {PackageId} does not exist", packageId);
            return null;
        }

        try
        {

            var iconFileStream = File.Open(iconPath, FileMode.Open, FileAccess.Read, FileShare.None);
            iconFileStream.Seek(0, SeekOrigin.Begin);
            return iconFileStream;
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Cannot open icon for {PackageId}", packageId);
        }

        return null;
    }

    public async Task CountDownloadAsync(string packageId, string version)
    {
        await _extensionRepository.CountDownloadAsync(packageId, version);
    }

    public async Task<long> GetDownloadCountAsync(string packageId)
    {
        var count = await _extensionRepository.GetDownloadCountAsync(packageId);
        return count;
    }

    public async Task<ExtensionDownloadInfo[]> GetDownloadsAsync(string packageId, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var downloadInfoEntities = await _extensionRepository.GetDownloadsAsync(packageId, startDate, endDate);
        if (downloadInfoEntities.Length == 0)
        {
            return Array.Empty<ExtensionDownloadInfo>();
        }

        var downloadInfos = _mapper.Map<ExtensionDownloadInfo[]>(downloadInfoEntities);
        return downloadInfos;
    }
}