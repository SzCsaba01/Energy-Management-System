using Device.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Project.Data.Data;
using Project.Data.Data.Entities;

namespace Device.Data.Access;
public class DeviceRepository : IDeviceRepository {
    private readonly DataContext _dataContext;
    public DeviceRepository(DataContext dataContext) {
        _dataContext = dataContext;
    }
    public async Task AddDeviceAsync(DeviceEntity device) {
        await _dataContext.Devices.AddAsync(device);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<ICollection<DeviceEntity>> GetAllDevicesAsync() {
        return await _dataContext.Devices.AsNoTracking().ToListAsync();
    }

    public async Task<DeviceEntity> GetDeviceByIdAsync(Guid id) {
        return await _dataContext.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(device => device.Id == id);
    }
    public async Task<DeviceEntity> GetDeviceByNameAsync(string name) {
        return await _dataContext.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(device => device.Name == name);
    }

    public async Task<ICollection<DeviceEntity>> GetDevicesByUserIdAsync(Guid userId) {
        return await _dataContext.Devices
            .Where(device => device.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<DeviceEntity>> GetUnassignedDevicesAsync()
    {
        return await _dataContext.Devices
            .Where(device => device.UserId == null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateDeviceAsync(DeviceEntity device) {
        _dataContext.Devices.Update(device);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteDeviceAsync(DeviceEntity device) {
        _dataContext.Devices.Remove(device);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveUserFromAllDevicesAsync(ICollection<DeviceEntity> devices) {
        _dataContext.Devices.UpdateRange(devices);
        await _dataContext.SaveChangesAsync();
    }
}
