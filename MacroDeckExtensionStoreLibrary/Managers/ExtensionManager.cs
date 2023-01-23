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
    private readonly IMapper _mapper;

    public ExtensionManager(IExtensionRepository extensionRepository, IMapper mapper)
    {
        _extensionRepository = extensionRepository;
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
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Failed to create extension");
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
            _logger.Fatal(ex, "Failed to delete extension - packageId {PackageId}", packageId);
        }
    }
}