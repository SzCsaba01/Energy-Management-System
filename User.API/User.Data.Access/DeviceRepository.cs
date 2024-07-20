using Project.Data.Data;
using User.Data.Contracts;
using User.Data.Object.Entities;

namespace User.Data.Access;
public class DeviceRepository : IDeviceRepository {
    private readonly DataContext _dataContext;
    public DeviceRepository(DataContext dataContext) {
        _dataContext = dataContext;
    }
    public async Task AddDeviceAsync(DeviceEntity device) {
        await _dataContext.Devices.AddAsync(device);
    }

    public async Task DeleteDeviceAsync(DeviceEntity device) {
        _dataContext.Devices.Remove(device);
    }
}
