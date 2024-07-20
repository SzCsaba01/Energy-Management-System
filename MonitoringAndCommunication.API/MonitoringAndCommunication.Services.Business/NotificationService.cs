using Microsoft.AspNetCore.SignalR;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Notification;
using MonitoringAndCommunication.Services.Contracts;

namespace MonitoringAndCommunication.Services.Business;
public class NotificationService : Hub, INotificationService
{
    private readonly IHubContext<NotificationService> _hubContext;
    public NotificationService(IHubContext<NotificationService> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendDeviceNotificationAsync(NotificationDto notificationDto)
    {
       await _hubContext.Clients.Group(notificationDto.DeviceId.ToString()).SendAsync("ReceiveDeviceNotification", notificationDto.Message);
    }

    public async Task JoinGroupAsync(Guid deviceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, deviceId.ToString());
    }

    public async Task LeaveGroupAsync(Guid deviceId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceId.ToString());
    }
}
