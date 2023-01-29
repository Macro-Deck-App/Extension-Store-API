using AutoMapper;
using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.Models;
using MacroDeckExtensionStoreLibrary.Models.ApiV2;

namespace MacroDeckExtensionStoreLibrary.Automapper;

[UsedImplicitly]
public class ApiV2Profile : Profile
{
    public ApiV2Profile()
    {
        CreateMap<Extension, ApiV2Extension>()
            .ReverseMap();
        CreateMap<ExtensionDownloadInfo, ApiV2ExtensionDownloadInfo>()
            .ReverseMap();
        CreateMap<ExtensionFile, ApiV2ExtensionFile>()
            .ReverseMap();
        CreateMap<ExtensionSummary, ApiV2ExtensionSummary>()
            .ReverseMap();
    }
}