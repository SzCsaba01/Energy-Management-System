using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;

namespace User.Services.Contracts;
public interface ICommunicationService {
    public Task AssignUserToDeviceAsync(UserToDeviceDto userToDeviceDto);
    public Task RemoveUserFromDeviceAsync(UserToDeviceDto userToDeviceDto);
    public Task RemoveUserFromAllDevicesByUserIdAsync(Guid userId);
    public Task<List<DeviceDto>> GetDevicesByUserIdAsync(Guid id);
}
