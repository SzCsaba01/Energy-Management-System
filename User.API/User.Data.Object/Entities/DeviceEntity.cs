using Project.Data.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Data.Object.Entities;
[Table("Devices")]
public class DeviceEntity {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}
