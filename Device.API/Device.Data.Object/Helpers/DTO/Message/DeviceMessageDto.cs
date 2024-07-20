using Device.Data.Objects.Helpers.DTO.Device;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Data.Objects.Helpers.DTO.Message;
public class DeviceMessageDto
{
    public DeviceToMonitoringDto Device { get; set; }
    public string MessageType { get; set; }
}
