using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.MacroDeck;
using ExtensionStoreAPI.Core.DataTypes.Response;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.AutoMapper;

[UsedImplicitly]
public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<ExtensionEntity, Extension>();

        CreateMap<Extension, ExtensionEntity>()
            .ForMember(dest => dest.Downloads, opt => opt.Ignore());
        
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));
        
        CreateMap<ExtensionEntity, ExtensionSummary>()
            .ForMember(dest => dest.TotalDownloads, opt => opt.Ignore());

        CreateMap<Extension, ExtensionSummary>()
            .ReverseMap();
        
        CreateMap<ExtensionManifest, Extension>()
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.Type))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.Repository))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.AuthorDiscordUserId))
            .ReverseMap();

        CreateMap<ExtensionDownloadInfoEntity, ExtensionDownloadInfo>()
            .ReverseMap();
    }
}