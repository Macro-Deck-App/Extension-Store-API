using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.Models;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.Core.Automapper;

[UsedImplicitly]
public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<ExtensionEntity, Extension>()
            .ForMember(dest => dest.TotalDownloads, opt => opt.MapFrom(x => x.Downloads.Count));

        CreateMap<Extension, ExtensionEntity>()
            .ForMember(dest => dest.Downloads, opt => opt.Ignore());
        
        CreateMap(typeof(PagedData<>), typeof(PagedData<>));
        
        CreateMap<ExtensionEntity, ExtensionSummary>()
            .ForMember(dest => dest.TotalDownloads, opt => opt.MapFrom(x => x.Downloads.Count));

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