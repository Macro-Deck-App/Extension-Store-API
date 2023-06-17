using AutoMapper;
using ExtensionStoreAPI.Core.Models;
using ExtensionStoreAPI.Core.Models.ApiV2;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.Core.Automapper;

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