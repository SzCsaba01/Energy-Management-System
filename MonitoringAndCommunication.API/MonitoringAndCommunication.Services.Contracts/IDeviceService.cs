using MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Message;

namespace MonitoringAndCommunication.Services.Contracts;
public interface IDeviceService
{
    public Task AddDeviceAsync(DeviceToMonitoringDto deviceToMonitoringDto);
    public Task UpdateDeviceAsync(DeviceToMonitoringDto deviceToMonitoringDto);
    public Task RemoveDeviceByIdAsync(Guid deviceId);
}
