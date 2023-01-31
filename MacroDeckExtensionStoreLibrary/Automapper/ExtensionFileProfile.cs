using AutoMapper;
using JetBrains.Annotations;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Automapper;

[UsedImplicitly]
public class ExtensionFileProfile : Profile
{
    public ExtensionFileProfile()
    {
        CreateMap<ExtensionFileEntity, ExtensionFile>()
            .ReverseMap();

        CreateMap<ExtensionFileUploadResult, ExtensionFile>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.Md5Hash, opt => opt.MapFrom(x => x.Md5));

        CreateMap<ExtensionFileUploadResult, ExtensionFileEntity>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.Md5Hash, opt => opt.MapFrom(x => x.Md5));
    }
}