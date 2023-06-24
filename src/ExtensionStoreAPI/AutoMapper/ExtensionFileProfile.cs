using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
using ExtensionStoreAPI.Core.DataTypes.Response;
using JetBrains.Annotations;

namespace ExtensionStoreAPI.AutoMapper;

[UsedImplicitly]
public class ExtensionFileProfile : Profile
{
    public ExtensionFileProfile()
    {
        CreateMap<ExtensionFileEntity, ExtensionFile>()
            .ForMember(dest => dest.UploadDateTime, opt => opt.MapFrom(x => x.CreatedTimestamp))
            .ReverseMap();

        CreateMap<ExtensionFileUploadResult, ExtensionFile>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.FileHash, opt => opt.MapFrom(x => x.FileHash));

        CreateMap<ExtensionFileUploadResult, ExtensionFileEntity>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(x => x.ExtensionManifest!.Version))
            .ForMember(dest => dest.MinApiVersion, opt => opt.MapFrom(x => x.ExtensionManifest!.TargetPluginApiVersion))
            .ForMember(dest => dest.FileHash, opt => opt.MapFrom(x => x.FileHash));
    }
}