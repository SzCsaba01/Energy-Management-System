using AutoMapper;
using Device.Data.Contracts;
using Device.Data.Objects.Helpers.DTO.Device;
using Device.Data.Objects.Helpers.DTO.Message;
using Device.Services.Contracts;
using Project.Data.Data.Entities;
using System.ComponentModel.DataAnnotations;
using User.Data.Contracts.Helpers.DTO.Device;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Services.Business;
public class DeviceService : IDeviceService {
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly ICommunicationService _communicationService;
    private readonly IDeviceMessageQueueService _messageQueueService;
    public DeviceService(
        IDeviceRepository deviceRepository, 
        IMapper mapper,
        ICommunicationService communicationService,
        IDeviceMessageQueueService messageQueueService
        )
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _communicationService = communicationService;
        _messageQueueService = messageQueueService;
    }
    public async Task<DeviceDto> AddDeviceAsync(DeviceDto deviceDto) {
        var device = _mapper.Map<DeviceEntity>(deviceDto);

        var deviceWithSameName = await _deviceRepository.GetDeviceByNameAsync(device.Name);

        if (deviceWithSameName != null) {
            throw new Exception("Device with same name already exists!");
        }

        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(device, new ValidationContext(device), results, validateAllProperties: true);
        var errorMessages = results.Select(x => x.ErrorMessage);

        if (!valid) {
            throw new Exception(string.Join(" ", errorMessages));
        }

        await _deviceRepository.AddDeviceAsync(device);

        var deviceToMonitoringDto = _mapper.Map<DeviceToMonitoringDto>(device);
        var messageDto = new DeviceMessageDto
        {
            Device = deviceToMonitoringDto,
            MessageType = "Add"
        };

        await Task.Run(() => _messageQueueService.SendMessage(messageDto));
        return _mapper.Map<DeviceDto>(device);
    }
    public async Task<ICollection<DeviceDto>> GetDevicesByUserId(Guid userId) {
        var devices = await _deviceRepository.GetDevicesByUserIdAsync(userId);

        if (devices == null) {
            throw new Exception("Devices not found!");
        }

        var deviceDtos = _mapper.Map<ICollection<DeviceEntity>, ICollection<DeviceDto>>(devices);

        return deviceDtos;
    }

    public async Task<ICollection<DeviceDto>> GetUnassignedDevices()
    {
        var devices = await _deviceRepository.GetUnassignedDevicesAsync();

        var deviceDtos = _mapper.Map<ICollection<DeviceEntity>, ICollection<DeviceDto>>(devices);

        return deviceDtos;
    }

    public async Task<ICollection<DeviceDto>> GetAllDevices() {
        var devices = await _deviceRepository.GetAllDevicesAsync();
        if (devices == null) {
              throw new Exception("Devices not found!");
        }

        var deviceDtos = _mapper.Map<ICollection<DeviceEntity>, ICollection<DeviceDto>>(devices);
        return deviceDtos;
    }

    public async Task AssignUserToDeviceAsync(UserToDeviceDto deviceToUserDto) {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceToUserDto.DeviceId);

        if (device == null) {
            throw new Exception("Device not found!");
        }

        var devices = await _deviceRepository.GetDevicesByUserIdAsync(deviceToUserDto.UserId);
        var deviceWithSameAddress = devices.FirstOrDefault(x => x.Address == device.Address);

        if (deviceWithSameAddress != null) {
            throw new Exception("User already has a device with the same address!");
        }

        device.UserId = deviceToUserDto.UserId;

        await _deviceRepository.UpdateDeviceAsync(device);
    }

    public async Task RemoveUserFromDeviceAsync(UserToDeviceDto deviceToUserDto)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceToUserDto.DeviceId);

        if (device == null)
        {
            throw new Exception("Device not found!");
        }

        device.UserId = null;

        await _deviceRepository.UpdateDeviceAsync(device);
    }

    public async Task RemoveUserFromAllDevicesByUserIdAsync(Guid userId)
    {
        var devices = await _deviceRepository.GetDevicesByUserIdAsync(userId);

        if (devices == null)
        {
            throw new Exception("Devices not found!");
        }

        foreach (var device in devices)
        {
            device.UserId = null;
        }

        await _deviceRepository.RemoveUserFromAllDevicesAsync(devices);
    }

    public async Task UpdateDeviceAsync(DeviceDto deviceDto) {
        var oldDevice = await _deviceRepository.GetDeviceByIdAsync(deviceDto.Id);

        if (oldDevice == null) {               
            throw new Exception("Device not found!");
        }

        if (deviceDto.Name != oldDevice.Name) {
            var devices = await _deviceRepository.GetDeviceByNameAsync(deviceDto.Name);

            if (devices != null) {
                  throw new Exception("Device with the same name already exists!");
            }
        }

        var updatedDevice = _mapper.Map<DeviceEntity>(deviceDto);

        if (oldDevice.MaxHourlyEnergyConsumption != updatedDevice.MaxHourlyEnergyConsumption)
        {
            var deviceToMonitoringDto = _mapper.Map<DeviceToMonitoringDto>(updatedDevice);
            var messageDto = new DeviceMessageDto
            {
                Device = deviceToMonitoringDto,
                MessageType = "Update"
            };

            await Task.Run(() => _messageQueueService.SendMessage(messageDto));
        }

        await _deviceRepository.UpdateDeviceAsync(updatedDevice);
    }

    public async Task DeleteDeviceByIdAsync(Guid id) {
        var device = await _deviceRepository.GetDeviceByIdAsync(id);

        if (device == null) {
              throw new Exception("Device not found!");
        }

        if (device.UserId != null) {
            await _communicationService.RemoveDeviceFromUserAsync(new UserToDeviceDto {
                UserId = device.UserId.Value,
                DeviceId = device.Id
            });
        }

        var deviceToMonitoringDto = _mapper.Map<DeviceToMonitoringDto>(device);
        var messageDto = new DeviceMessageDto
        {
            Device = deviceToMonitoringDto,
            MessageType = "Delete"
        };

        await Task.Run (() => _messageQueueService.SendMessage(messageDto));

        await _deviceRepository.DeleteDeviceAsync(device);
    }
}
