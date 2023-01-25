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
            .ForMember(dest => dest.PackageFileName, opt => opt.MapFrom(x => x.PackageFileName))
            .ForMember(dest => dest.IconFileName, opt => opt.MapFrom(x => x.IconFileName))
            .ForMember(dest => dest.Md5Hash, opt => opt.MapFrom(x => x.Md5));

        CreateMap<ExtensionFileUploadResult, ExtensionFileEntity>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.IconFileName, opt => opt.MapFrom(x => x.IconFileName))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.Md5Hash, opt => opt.MapFrom(x => x.Md5))
            .ForMember(dest => dest.PackageFileName, opt => opt.MapFrom(x => x.PackageFileName))
            .ForMember(dest => dest.ReadmeHtml, opt => opt.MapFrom(x => x.ReadmeHtml))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Description))
            .ForMember(dest => dest.LicenseName, opt => opt.MapFrom(x => x.LicenseName))
            .ForMember(dest => dest.LicenseUrl, opt => opt.MapFrom(x => x.LicenseUrl));
    }
}