using AutoMapper;
using Project.Data.Data.Entities;
using System.ComponentModel.DataAnnotations;
using User.Data.Contracts;
using User.Data.Contracts.Helpers.DTO.User;
using User.Data.Object.Entities;
using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;
using User.Services.Contracts;

namespace User.Services.Business;
public class UserService : IUserService {
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ICommunicationService _communicationService;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IDeviceRepository deviceRepository,
        IEncryptionService encryptionService,
        ICommunicationService communicationService,
        IMapper mapper) {
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _communicationService = communicationService;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }

    public async Task AddUserAsync(UserDto userDto) {
        var user = _mapper.Map<UserEntity>(userDto);
        user.Role = "User";

        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(user, new ValidationContext(user), results, validateAllProperties: true);
        var errorMessages = results.Select(x => x.ErrorMessage);

        if (!valid) {
            throw new Exception(string.Join(" ", errorMessages));
        }

        user.Password = _encryptionService.GeneratedHashedPassword(user.Password);

        await _userRepository.AddUserAsync(user);
    }

    public async Task<Guid> GetAdminIdAsync()
    {
        var admin = await _userRepository.GetAdminAsync();

        if (admin == null)
        {
            throw new Exception("Admin not found!");
        }

        return admin.Id;
    }

    public async Task<ICollection<string>> GetUsernamesAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        if (users is null)
        {
            throw new Exception("No users found!");
        }

        var usernames = users.Select(x => x.Username).ToList();

        return usernames;
    }

    public async Task<ICollection<UserWithDevicesDto>> GetAllUsersAsync() {
        var users = await _userRepository.GetAllUsersAsync();

        if (users is null) {
            throw new Exception("No users found!");
        }

        var userDtos = new List<UserWithDevicesDto>();

        foreach (var user in users) {
            var userDto = _mapper.Map<UserWithDevicesDto>(user);
            userDtos.Add(userDto);
            var userDevices = await _communicationService.GetDevicesByUserIdAsync(user.Id);
            userDto.Devices = userDevices;
        }

        return userDtos;
    }

    public async Task AssignDeviceToUserAsync(DeviceToUserDto deviceToUserDto) {
        var user = await _userRepository.GetUserByUsernameWithDevicesAsync(deviceToUserDto.Username);

        if (user == null) {
            throw new Exception("User not found!");
        }

        var userToDevice = new UserToDeviceDto {
            UserId = user.Id,
            DeviceId = deviceToUserDto.DeviceId
        };

        var device = _mapper.Map<DeviceEntity>(deviceToUserDto);

        await _communicationService.AssignUserToDeviceAsync(userToDevice);

        await _deviceRepository.AddDeviceAsync(device);

        user.Devices.Add(device);

        await _userRepository.UpdateUsersAsync(user);
    }

    public async Task RemoveDeviceFromUserByUsernameAndDeviceIdAsync(DeviceToUserDto deviceToUserDto) {
        var user = await _userRepository.GetUserByUsernameWithDevicesAsync(deviceToUserDto.Username);

        if (user == null) {
            throw new Exception("User not found!");
        }

        var device = user.Devices.FirstOrDefault(x => x.Id == deviceToUserDto.DeviceId);

        await _deviceRepository.DeleteDeviceAsync(device);

        user.Devices.Remove(device);

        var userToDevice = new UserToDeviceDto {
            UserId = user.Id,
            DeviceId = deviceToUserDto.DeviceId
        };

        await _communicationService.RemoveUserFromDeviceAsync(userToDevice);
        await _userRepository.UpdateUsersAsync(user);
    }

    public async Task RemoveDeviceFromUserAsync(UserToDeviceDto userToDeviceDto) {
        var user = await _userRepository.GetUserByUserIdAsync(userToDeviceDto.UserId);

        if (user == null) {
            throw new Exception("User not found!");
        }

        var device = user.Devices.FirstOrDefault(x => x.Id == userToDeviceDto.DeviceId);

        if (device == null) {
            throw new Exception("Device not found!");
        }

        await _deviceRepository.DeleteDeviceAsync(device);

        user.Devices.Remove(device);

        await _userRepository.UpdateUsersAsync(user);
    }

    public async Task UpdateUserAsync(UserDto userDto) {
        var user = await _userRepository.GetUserByUsernameAsync(userDto.Username);

        if (user == null) {
            throw new Exception("User not found!");
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.Password = userDto.Password;

        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(user, new ValidationContext(user), results, validateAllProperties: true);
        var errorMessages = results.Select(x => x.ErrorMessage);

        if (!valid) {
            throw new Exception(string.Join(" ", errorMessages));
        }

        user.Password = _encryptionService.GeneratedHashedPassword(userDto.Password);

        await _userRepository.UpdateUsersAsync(user);
    }

    public async Task DeleteUserByUsernameAsync(string username) {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) {
            throw new Exception("User not found!");
        }

        await _communicationService.RemoveUserFromAllDevicesByUserIdAsync(user.Id);
        await _userRepository.DeleteUserByUsernameAsync(user);
    }
}
