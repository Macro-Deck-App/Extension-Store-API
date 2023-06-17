using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.Models;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.Core.Automapper;

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
            .ForMember(dest => dest.Md5Hash, opt => opt.MapFrom(x => x.FileHash));

        CreateMap<ExtensionFileUploadResult, ExtensionFileEntity>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.FileHash, opt => opt.MapFrom(x => x.FileHash));
    }
}