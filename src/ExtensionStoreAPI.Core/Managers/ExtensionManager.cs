using System.Globalization;
using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataAccess.RepositoryInterfaces;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Request;
using ExtensionStoreAPI.Core.DataTypes.Response;
using ExtensionStoreAPI.Core.Exceptions;
using ExtensionStoreAPI.Core.ManagerInterfaces;
using JetBrains.Annotations;
using Serilog;

namespace ExtensionStoreAPI.Core.Managers;

[UsedImplicitly]
public class ExtensionManager : IExtensionManager
{
    private readonly ILogger _logger = Log.ForContext<ExtensionManager>();
    private readonly IExtensionRepository _extensionRepository;
    private readonly IExtensionFileRepository _extensionFileRepository;
    private readonly IExtensionDownloadInfoRepository _extensionDownloadInfoRepository;
    private readonly IMapper _mapper;

    public ExtensionManager(
        IExtensionRepository extensionRepository,
        IExtensionFileRepository extensionFileRepository,
        IExtensionDownloadInfoRepository extensionDownloadInfoRepository,
        IFileManager fileManager,
        IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _extensionFileRepository = extensionFileRepository;
        _extensionDownloadInfoRepository = extensionDownloadInfoRepository;
        _mapper = mapper;
    }

    public async ValueTask<Extension> GetByPackageIdAsync(string packageId)
    {
        var extensionEntity = await _extensionRepository.GetByPackageIdAsync(packageId);
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        var extension = _mapper.Map<Extension>(extensionEntity);
        return extension;
    }

    public async ValueTask<ExtensionSummary> GetSummaryByPackageIdAsync(string packageId)
    {
        var extensionEntity = await _extensionRepository.GetByPackageIdAsync(packageId);
        if (extensionEntity == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }

        var extension = _mapper.Map<ExtensionSummary>(extensionEntity);
        return extension;
    }

    public async ValueTask<string[]> GetCategoriesAsync(Filter filter)
    {
        return await _extensionRepository.GetCategoriesAsync(filter);
    }

    public async ValueTask<List<ExtensionSummary>> GetTopDownloadsOfMonth(Filter filter, int month, int year, int count)
    {
        var topEntities = await _extensionDownloadInfoRepository.GetTopDownloadsOfMonth(filter, month, year, count);
        return _mapper.Map<List<ExtensionSummary>>(topEntities);
    }

    public async ValueTask<bool> ExistsAsync(string packageId)
    {
        var exists = await _extensionRepository.ExistAsync(packageId);
        return exists;
    }

    public async ValueTask<PagedList<ExtensionSummary>> GetAllAsync(string? searchString, Filter? filter, Pagination pagination)
    {
        var extensionEntities = await _extensionRepository.GetAllAsync(searchString, filter, pagination);
        return _mapper.Map<PagedList<ExtensionSummary>>(extensionEntities);
    }

    public async ValueTask CreateAsync(Extension extension)
    {
        try
        {
            var extensionEntity = _mapper.Map<ExtensionEntity>(extension);
            extensionEntity.Category = FixCategoryName(extensionEntity.Category);
            await _extensionRepository.CreateExtensionAsync(extensionEntity);
            _logger.Information("Created extension {PackageId}", extension.PackageId);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Failed to create extension {PackageId}", extension.PackageId);
        }
    }

    public async ValueTask<FileStream> GetIconStreamAsync(string packageId)
    {
        var extensionFile = await _extensionFileRepository.GetFileAsync(packageId);
        if (extensionFile == null)
        {
            throw ErrorCodeExceptions.PackageIdNotFoundException();
        }
        var iconPath = Path.Combine(Paths.DataDirectory, extensionFile.IconFileName);
        if (!File.Exists(iconPath))
        {
            _logger.Fatal("Icon file for {PackageId} does not exist", packageId);
            throw ErrorCodeExceptions.IconNotFoundException();
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
            throw ErrorCodeExceptions.InternalErrorException();
        }
    }

    private static string FixCategoryName(string? categoryName)
    {
        return string.IsNullOrWhiteSpace(categoryName)
            ? "No Category"
            : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categoryName.Trim());
    }
}