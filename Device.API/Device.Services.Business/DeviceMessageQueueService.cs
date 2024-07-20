using Device.Data.Objects.Helpers.DTO.Message;
using Device.Services.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Device.Services.Business;
public class DeviceMessageQueueService : IDeviceMessageQueueService, IDisposable
{
    private IConnection _connection;
    private IModel _channel;

    public DeviceMessageQueueService()
    {
        //HostName = "localhost",
        //HostName = device-queue
        var factory = new ConnectionFactory { HostName = "device-queue", Port = 5672 };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "devices", type: ExchangeType.Fanout);
    }

    public void SendMessage(DeviceMessageDto message)
    {
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "devices", "", null, body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
