using User.Data.Contracts.Helpers.DTO.User;
using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;

namespace User.Services.Contracts;
public interface IUserService {
    public Task AddUserAsync(UserDto userDto);
    public Task<Guid> GetAdminIdAsync();
    public Task<ICollection<string>> GetUsernamesAsync();
    public Task<ICollection<UserWithDevicesDto>> GetAllUsersAsync();
    public Task UpdateUserAsync(UserDto userDto);
    public Task AssignDeviceToUserAsync(DeviceToUserDto deviceToUserDto);
    public Task RemoveDeviceFromUserByUsernameAndDeviceIdAsync(DeviceToUserDto deviceToUserDto);
    public Task RemoveDeviceFromUserAsync(UserToDeviceDto userToDeviceDto);
    public Task DeleteUserByUsernameAsync(string username);
}
