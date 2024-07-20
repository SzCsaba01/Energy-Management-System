using AutoMapper;
using Project.Data.Data.Entities;
using User.Data.Contracts.Helpers.DTO.User;
using User.Data.Object.Entities;
using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;

namespace User.Data.Contracts.Helpers;
public class Mapper: Profile{

    public Mapper() {
        CreateMap<UserDto, UserEntity>();
        CreateMap<UserEntity, UserDto>();

        CreateMap<UserEntity, AuthenticationDto>()
            .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.Id));

        CreateMap<UserWithDevicesDto, UserEntity>();
        CreateMap<UserEntity, UserWithDevicesDto>()
            .ForMember(dest => dest.Devices, options => options.Ignore());

        CreateMap<DeviceToUserDto, DeviceEntity>()
            .ForMember(dest => dest.Id, options => options.MapFrom(src => src.DeviceId));

        CreateMap<UserToDeviceDto, DeviceEntity>()
            .ForMember(dest => dest.Id, options => options.MapFrom(src => src.DeviceId))
            .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId));
    }

}
