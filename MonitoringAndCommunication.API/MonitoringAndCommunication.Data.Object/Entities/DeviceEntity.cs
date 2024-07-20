using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringAndCommunication.Data.Object.Entities;
[Table("Devices")]
public class DeviceEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Device name is required")]
    [MaxLength(50, ErrorMessage = "Device name can have max 50 characters")]
    [MinLength(5, ErrorMessage = "Device name must contain atleast 5 characters")]
    public string Name { get; set; }

    public ICollection<MonitoringEntity>? Monitorings { get; set; }

    [Required]
    public int MaxHourlyEnergyConsumption { get; set; }

    [Required]
    public double CurrentHourEnergyConsumption { get; set; }

    [Required]
    public double TotalEnergyConsumption { get; set; }

    [Range(0, 6)]
    public int CurrentNumberOfMeasurments { get; set; }
}
