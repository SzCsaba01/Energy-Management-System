using Device.Data.Objects.Helpers.DTO.Message;

namespace Device.Services.Contracts;
public interface IDeviceMessageQueueService
{
    void SendMessage(DeviceMessageDto message);
}
