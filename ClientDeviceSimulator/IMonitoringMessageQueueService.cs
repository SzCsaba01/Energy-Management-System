namespace ClientDeviceSimulator;
public interface IMonitoringMessageQueueService
{
    public void SendMessage(MonitoringDto monitoringDto);
}
