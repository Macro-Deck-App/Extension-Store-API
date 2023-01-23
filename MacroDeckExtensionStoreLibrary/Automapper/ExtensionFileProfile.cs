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
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest.TargetPluginAPIVersion))
            .ForMember(dest => dest.PackageFileName, opt => opt.MapFrom(x => x.PackageFileName))
            .ForMember(dest => dest.IconFileName, opt => opt.MapFrom(x => x.IconFileName))
            .ForMember(dest => dest.MD5Hash, opt => opt.MapFrom(x => x.MD5));
    }
}