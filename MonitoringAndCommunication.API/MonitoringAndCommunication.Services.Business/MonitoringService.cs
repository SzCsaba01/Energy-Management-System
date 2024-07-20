using AutoMapper;
using Microsoft.AspNet.SignalR;
using MonitoringAndCommunication.Data.Contracts;
using MonitoringAndCommunication.Data.Object.Entities;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Notification;
using MonitoringAndCommunication.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MonitoringAndCommunication.Services.Business;
public class MonitoringService : IMonitoringService
{
    private readonly IMonitoringRepository _monitoringRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    public MonitoringService(
        IMonitoringRepository monitoringRepository, 
        IMapper mapper, 
        IDeviceRepository deviceRepository,
        INotificationService notificationService)
    {
        _deviceRepository = deviceRepository;
        _monitoringRepository = monitoringRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }
    public async Task AddMonitoring(MonitoringDto monitoringDto)
    {
        var monitoring = _mapper.Map<MonitoringEntity>(monitoringDto);

        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(monitoring, new ValidationContext(monitoring), results, validateAllProperties: true);
        var errorMessages = results.Select(x => x.ErrorMessage);

        if (!valid)
        {
            throw new Exception(string.Join(" ", errorMessages));
        }

        var device = await _deviceRepository.GetDeviceByIdAsync(monitoringDto.DeviceId);

        if (device is null) 
        {
            throw new Exception("Device not found!");
        }

        var lastMonitoring = device.Monitorings.LastOrDefault();

        if (lastMonitoring is not null && device.CurrentNumberOfMeasurments > 0)
        {
            device.CurrentHourEnergyConsumption += Math.Abs(lastMonitoring.MeasurmentValue - monitoringDto.MeasurmentValue);
            monitoring.Timestamp = lastMonitoring.Timestamp.AddMinutes(10);

            if (device.CurrentHourEnergyConsumption > device.MaxHourlyEnergyConsumption)
            {
                var notificationDto = new NotificationDto
                {
                    DeviceId = device.Id,
                    Message = $"Max hourly energy consumption exceeded for {device.Name}!"
                };

                await _notificationService.SendDeviceNotificationAsync(notificationDto);
            }
        }
        else
        {
            device.CurrentHourEnergyConsumption = monitoring.MeasurmentValue;
            monitoring.Timestamp = DateTimeOffset.Now;
        }

        device.CurrentNumberOfMeasurments++;

        if (device.CurrentNumberOfMeasurments >= 6)
        {
            device.TotalEnergyConsumption += device.CurrentHourEnergyConsumption;
            device.CurrentNumberOfMeasurments = 0;
            device.CurrentHourEnergyConsumption = 0;
        }

        await _deviceRepository.UpdateDeviceAsync(device);
        await _monitoringRepository.AddMonitoringAsync(monitoring);
    }

    public async Task<List<MonitoringDto>> GetMonitoringsByDeviceIds(ICollection<Guid> deviceIds)
    {
        var devices = await _deviceRepository.GetDevicesWithMonitoringByDeviceIdsAsync(deviceIds);

        var monitoringsDto = new List<MonitoringDto>();
        foreach(var device in devices)
        {
            monitoringsDto.AddRange(device.Monitorings
                .Select(x => new MonitoringDto
                {
                    DeviceId = device.Id,
                    DeviceName = device.Name,
                    MeasurmentValue = x.MeasurmentValue,
                    Timestamp = x.Timestamp
                }).ToList());
        }

        return monitoringsDto;
    }
}
