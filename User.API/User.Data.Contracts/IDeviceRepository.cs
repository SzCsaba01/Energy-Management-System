using User.Data.Object.Entities;

namespace User.Data.Contracts;
public interface IDeviceRepository {
    public Task AddDeviceAsync(DeviceEntity device);
    public Task DeleteDeviceAsync(DeviceEntity device);
}
