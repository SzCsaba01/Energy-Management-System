using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringAndCommunication.Data.Object.Entities;
[Table("Monitorings")]
public class MonitoringEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTimeOffset Timestamp { get; set; }

    [Required]
    public double MeasurmentValue { get; set; }
    [Required]

    public Guid DeviceId { get; set; }
    public DeviceEntity Device { get; set; }
}
