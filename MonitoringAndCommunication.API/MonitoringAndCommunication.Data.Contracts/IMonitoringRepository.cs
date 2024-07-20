using MonitoringAndCommunication.Data.Object.Entities;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;

namespace MonitoringAndCommunication.Data.Contracts;
public interface IMonitoringRepository
{
    public Task AddMonitoringAsync(MonitoringEntity monitoringEntity);
}
