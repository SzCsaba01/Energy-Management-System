using Microsoft.EntityFrameworkCore;
using Project.Data.Data;
using Project.Data.Data.Entities;
using User.Data.Contracts;
using User.Data.Object.Entities;

namespace User.Data.Access;
public class UserRepository : IUserRepository{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext) {
        _dataContext = dataContext;
    }

    public async Task AddUserAsync(UserEntity user) {
        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<UserEntity> GetAdminAsync()
    {
        return await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Role == "Admin");
    }

    public async Task<List<UserEntity>> GetAllUsersAsync() {
        return await _dataContext.Users
            .Where(u => u.Role != "Admin")
            .Include(u => u.Devices)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserEntity> GetUserByUsernameAsync(string username) {
        return await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserEntity> GetUserByUsernameWithDevicesAsync(string username) {
        return await _dataContext.Users
            .Include(u => u.Devices)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserEntity> GetUserByUserIdAsync(Guid userId) {
        return await _dataContext.Users
            .Include(u => u.Devices)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateUsersAsync(UserEntity user) {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteUserByUsernameAsync(UserEntity user) {
        _dataContext.Users.Remove(user);
        await _dataContext.SaveChangesAsync();
    }
}
