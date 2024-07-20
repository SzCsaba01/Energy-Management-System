using User.Data.Object.Helpers.DTO.Device;

namespace User.Data.Object.Helpers.DTO.User;
public class UserWithDevicesDto {
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public List<DeviceDto> Devices { get; set; }
}
