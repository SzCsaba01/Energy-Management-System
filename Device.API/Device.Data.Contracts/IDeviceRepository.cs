using Device.Data.Objects.Helpers.DTO.Device;
using Project.Data.Data.Entities;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Data.Contracts;
public interface IDeviceRepository {
    public Task AddDeviceAsync(DeviceEntity device);
    public Task<DeviceEntity> GetDeviceByIdAsync(Guid id);
    public Task<DeviceEntity> GetDeviceByNameAsync(string name);
    public Task<ICollection<DeviceEntity>> GetUnassignedDevicesAsync();
    public Task<ICollection<DeviceEntity>> GetDevicesByUserIdAsync(Guid userId);
    public Task<ICollection<DeviceEntity>> GetAllDevicesAsync();
    public Task UpdateDeviceAsync(DeviceEntity device);
    public Task DeleteDeviceAsync(DeviceEntity device);
    public Task RemoveUserFromAllDevicesAsync(ICollection<DeviceEntity> devices);
}
