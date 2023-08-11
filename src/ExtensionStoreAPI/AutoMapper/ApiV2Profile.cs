using AutoMapper;
using ExtensionStoreAPI.Core.DataTypes.ApiV2;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.AutoMapper;

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