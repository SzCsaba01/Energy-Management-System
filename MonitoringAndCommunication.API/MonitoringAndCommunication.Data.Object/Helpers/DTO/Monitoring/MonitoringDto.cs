namespace MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;
public class MonitoringDto
{
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; }  
    public double MeasurmentValue { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
