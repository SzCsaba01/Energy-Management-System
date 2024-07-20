using Microsoft.EntityFrameworkCore;
using MonitoringAndCommunication.Data.Contracts;
using MonitoringAndCommunication.Data.Object.Data;
using MonitoringAndCommunication.Data.Object.Entities;

namespace MonitoringAndCommunication.Data.Access;
public class MonitoringRepository : IMonitoringRepository
{
    private readonly DataContext _dataContext;

    public MonitoringRepository (DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddMonitoringAsync(MonitoringEntity monitoringEntity)
    {
        await _dataContext.Monitorings.AddAsync(monitoringEntity);
        await _dataContext.SaveChangesAsync();
    }
}
