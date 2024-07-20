using MonitoringAndCommunication.Data.Object.Helpers.DTO.Notification;

namespace MonitoringAndCommunication.Services.Contracts;
public interface INotificationService
{
    public Task SendDeviceNotificationAsync(NotificationDto notificationDto);
    public Task JoinGroupAsync(Guid deviceId);
    public Task LeaveGroupAsync(Guid deviceId);
}
