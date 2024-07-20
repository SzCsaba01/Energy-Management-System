namespace MonitoringAndCommunication.Data.Object.Helpers.DTO.Device;
public class DeviceToMonitoringDto
{
    public Guid DeviceId { get; set; }
    public string Name { get; set; }
    public int MaxHourlyEnergyConsumption { get; set; }
}
