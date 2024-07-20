using AutoMapper;
using Device.Data.Objects.Helpers.DTO.Device;
using Project.Data.Data.Entities;
using User.Data.Contracts.Helpers.DTO.Device;

namespace User.Data.Contracts.Helpers;
public class Mapper: Profile{

    public Mapper() {
        CreateMap<DeviceDto, DeviceEntity>();
        CreateMap<DeviceEntity, DeviceDto>();
        CreateMap<DeviceEntity, DeviceToMonitoringDto>()
            .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MaxHourlyEnergyConsumption, opt => opt.MapFrom(src => src.MaxHourlyEnergyConsumption));
    }
}
