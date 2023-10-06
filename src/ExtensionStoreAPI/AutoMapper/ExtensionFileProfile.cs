using AutoMapper;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.ExtensionStore;
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
    }
}