using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.Data.Entities;
[Table("Devices")]
public class DeviceEntity {
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Device name is required")]
    [MaxLength(50, ErrorMessage = "Device name can have max 50 characters")]
    [MinLength(5, ErrorMessage = "Device name must contain atleast 5 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Device descriptions is required")]
    [MaxLength(200, ErrorMessage = "Device descriptions can have max 200 characters")]
    [MinLength(10, ErrorMessage = "Device descriptions must contain atleast 10 characters")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Device address is required")]
    [MaxLength(100, ErrorMessage = "Device address can have max 100 characters")]
    [MinLength(10, ErrorMessage = "Device address must contain atleast 10 characters")]
    public string Address { get; set; }
    [Required(ErrorMessage = "Device max hourly energy consumption is required")]
    [Range(5, 50, ErrorMessage = "Device max horuly consumption must be between 5 and 50")]
    public int MaxHourlyEnergyConsumption { get; set; }
    public Guid? UserId { get; set; }
}