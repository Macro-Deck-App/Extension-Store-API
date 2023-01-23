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
            .ReverseMap();
        CreateMap<Extension, ExtensionSummary>()
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(x => x.PackageId))
            .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(x => x.ExtensionType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(x => x.Author))
            .ForMember(dest => dest.GitHubRepository, opt => opt.MapFrom(x => x.GitHubRepository))
            .ForMember(dest => dest.DSupportUserId, opt => opt.MapFrom(x => x.DSupportUserId))
            .ForMember(dest => dest.Downloads, opt => opt.MapFrom(x => x.Downloads));
    }
}