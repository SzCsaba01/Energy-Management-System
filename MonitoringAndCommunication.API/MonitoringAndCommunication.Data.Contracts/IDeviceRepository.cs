using MonitoringAndCommunication.Data.Object.Entities;

namespace MonitoringAndCommunication.Data.Contracts;
public interface IDeviceRepository
{
    public Task AddDeviceAsync(DeviceEntity device);
    public Task<DeviceEntity> GetDeviceByIdAsync(Guid id);
    public Task<List<DeviceEntity>> GetDevicesWithMonitoringByDeviceIdsAsync(ICollection<Guid> deviceIds);
    public Task UpdateDeviceAsync(DeviceEntity device);
    public Task DeleteDeviceAsync(DeviceEntity device);
}
