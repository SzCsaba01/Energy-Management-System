using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using User.Data.Object.Entities;

namespace Project.Data.Data.Entities;
[Table("Users")]
public class UserEntity {
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(50, ErrorMessage = "Email can have max 50 characters")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username can have max 50 characters")]
    [MinLength(5, ErrorMessage = "Username must contain atleast 5 characters")]
    [RegularExpression("[a-zA-Z0-9._]+", ErrorMessage = "Username can contain only lower, upper cases and numbers")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[-+_!@#$%^&*.,?]).+$", ErrorMessage = "Password must contain atleast one lowercase, uppercase, number and special character")]
    public string Password { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(50, ErrorMessage = "First Name can have max 50 characters")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(50, ErrorMessage = "Last Name can have max 50 characters")]
    public string LastName { get; set; }
    public string Role { get; set; }
    public ICollection<DeviceEntity> Devices { get; set; }
}