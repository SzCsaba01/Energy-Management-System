using MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;

namespace MonitoringAndCommunication.Data.Object.Helpers.DTO.Message;
public class DeviceMessageDto
{
    public DeviceToMonitoringDto Device { get; set; }
    public string MessageType { get; set; }
}
