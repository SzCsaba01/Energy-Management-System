using Microsoft.EntityFrameworkCore;
using MonitoringAndCommunication.Data.Contracts;
using MonitoringAndCommunication.Data.Object.Data;
using MonitoringAndCommunication.Data.Object.Entities;

namespace MonitoringAndCommunication.Data.Access;
public class DeviceRepository : IDeviceRepository
{
    private readonly DataContext _dataContext;

    public DeviceRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddDeviceAsync(DeviceEntity device)
    {
        await _dataContext.Devices.AddAsync(device);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<List<DeviceEntity>> GetDevicesWithMonitoringByDeviceIdsAsync(ICollection<Guid> deviceIds)
    {
        return await _dataContext.Devices
            .Where(x => deviceIds.Contains(x.Id))
            .Include(x => x.Monitorings)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DeviceEntity> GetDeviceByIdAsync(Guid id)
    {
        return await _dataContext.Devices
            .Where(x => x.Id == id)
            .Include(x => x.Monitorings)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task UpdateDeviceAsync(DeviceEntity device)
    {
        _dataContext.Devices.Update(device);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteDeviceAsync(DeviceEntity device)
    {
        _dataContext.Devices.Remove(device);
        await _dataContext.SaveChangesAsync();
    }
}
