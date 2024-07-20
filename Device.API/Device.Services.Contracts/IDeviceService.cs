using User.Data.Contracts.Helpers.DTO.Device;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Services.Contracts;
public interface IDeviceService {
    public Task<DeviceDto> AddDeviceAsync(DeviceDto deviceDto);
    public Task<ICollection<DeviceDto>> GetDevicesByUserId(Guid userId);
    public Task<ICollection<DeviceDto>> GetAllDevices();
    public Task<ICollection<DeviceDto>> GetUnassignedDevices();
    public Task UpdateDeviceAsync(DeviceDto deviceDto);
    public Task AssignUserToDeviceAsync(UserToDeviceDto deviceToUserDto);
    public Task RemoveUserFromDeviceAsync(UserToDeviceDto deviceToUserDto);
    public Task RemoveUserFromAllDevicesByUserIdAsync(Guid userId);
    public Task DeleteDeviceByIdAsync(Guid id);
}
