using AutoMapper;
using MonitoringAndCommunication.Data.Contracts;
using MonitoringAndCommunication.Data.Object.Entities;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Message;
using MonitoringAndCommunication.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MonitoringAndCommunication.Services.Business;
public class DeviceService : IDeviceService
{
    private readonly IMapper _mapper;
    private readonly IDeviceRepository _deviceRepository;
    
    public DeviceService (IMapper mapper,IDeviceRepository deviceRepository)
    {
        _mapper = mapper;
        _deviceRepository = deviceRepository;
    }

    public async Task AddDeviceAsync(DeviceToMonitoringDto deviceToMonitoringDto)
    {
        var device = _mapper.Map<DeviceEntity>(deviceToMonitoringDto);

        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(device, new ValidationContext(device), results, validateAllProperties: true);
        var errorMessages = results.Select(x => x.ErrorMessage);

        if (!valid)
        {
            throw new Exception(string.Join(" ", errorMessages));
        }

        await _deviceRepository.AddDeviceAsync(device);
    }

    public async Task UpdateDeviceAsync(DeviceToMonitoringDto deviceToMonitoringDto)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceToMonitoringDto.DeviceId);

        if (device is null)
        {
            throw new Exception("Device not found!");
        }

        var updatedDevice = _mapper.Map(deviceToMonitoringDto, device);

        await _deviceRepository.UpdateDeviceAsync(updatedDevice);
    }

    public async Task RemoveDeviceByIdAsync(Guid deviceId)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceId);

        if (device is null)
        {
            throw new Exception("Device not found!");
        }

        await _deviceRepository.DeleteDeviceAsync(device);
    }
}
