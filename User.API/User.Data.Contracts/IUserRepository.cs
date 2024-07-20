using Project.Data.Data.Entities;
using User.Data.Contracts.Helpers.DTO.User;
using User.Data.Object.Entities;

namespace User.Data.Contracts;
public interface IUserRepository {
    public Task AddUserAsync(UserEntity user);
    public Task<UserEntity> GetAdminAsync();
    public Task<List<UserEntity>> GetAllUsersAsync();
    public Task<UserEntity> GetUserByUserIdAsync(Guid userId);
    public Task<UserEntity> GetUserByUsernameAsync(string username);
    public Task<UserEntity> GetUserByUsernameWithDevicesAsync(string username);
    public Task UpdateUsersAsync(UserEntity user);
    public Task DeleteUserByUsernameAsync(UserEntity user);
}
