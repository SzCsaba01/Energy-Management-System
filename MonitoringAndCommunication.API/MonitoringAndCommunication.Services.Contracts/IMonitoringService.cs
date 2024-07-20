using MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;

namespace MonitoringAndCommunication.Services.Contracts;
public interface IMonitoringService
{
    public Task AddMonitoring(MonitoringDto monitoringDto);
    public Task<List<MonitoringDto>> GetMonitoringsByDeviceIds(ICollection<Guid> deviceIds);
}
