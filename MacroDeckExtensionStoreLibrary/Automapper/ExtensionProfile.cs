using AutoMapper;
using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Automapper;

[UsedImplicitly]
public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<ExtensionEntity, Extension>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(x => x.Author))
            .ForMember(dest => dest.Downloads, opt => opt.MapFrom(x => x.Downloads.Count))
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.ExtensionType))
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(x => x.PackageId))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.GitHubRepository))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.DSupportUserId))
            .ForMember(dest => dest.ExtensionFiles, opt => opt.MapFrom(x => x.ExtensionFiles));

        CreateMap<ExtensionEntity, ExtensionSummary>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(x => x.Author))
            .ForMember(dest => dest.Downloads, opt => opt.MapFrom(x => x.Downloads.Count))
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.ExtensionType))
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(x => x.PackageId))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.GitHubRepository))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.DSupportUserId));

        CreateMap<Extension, ExtensionSummary>()
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(x => x.PackageId))
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.ExtensionType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(x => x.Author))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.GitHubRepository))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.DSupportUserId))
            .ForMember(dest => dest.Downloads, opt => opt.MapFrom(x => x.Downloads))
            .ReverseMap();
        
        CreateMap<ExtensionManifest, Extension>()
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.Type))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(x => x.Author))
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(x => x.PackageId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.Repository))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.AuthorDiscordUserId))
            .ReverseMap();

        CreateMap<ExtensionDownloadInfoEntity, ExtensionDownloadInfo>()
            .ForMember(dest => dest.DownloadedVersion, opt => opt.MapFrom(x => x.DownloadedVersion))
            .ForMember(dest => dest.DownloadDateTime, opt => opt.MapFrom(x => x.DownloadDateTime))
            .ReverseMap();
    }
}