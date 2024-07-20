using AutoMapper;
using MonitoringAndCommunication.Data.Object.Entities;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;

namespace MonitoringAndCommunication.Data.Object.Helpers;
public class Mapper : Profile
{

    public Mapper()
    {
        CreateMap<DeviceToMonitoringDto, DeviceEntity>()
            .ForMember(dest => dest.Id, options => options.MapFrom(src => src.DeviceId))
            .ForMember(dest => dest.MaxHourlyEnergyConsumption, options => options.MapFrom(src => src.MaxHourlyEnergyConsumption));

        CreateMap<MonitoringDto, MonitoringEntity>();
        CreateMap<MonitoringEntity, MonitoringDto>();
    }
}
