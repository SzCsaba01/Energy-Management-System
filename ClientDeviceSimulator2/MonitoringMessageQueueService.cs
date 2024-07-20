using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ClientDeviceSimulator;
public class MonitoringMessageQueueService : IMonitoringMessageQueueService, IDisposable
{
    private IConnection _connection;
    private IModel _channel;

    public MonitoringMessageQueueService()
    {
        var factory = new ConnectionFactory { HostName = "localhost", Port = 5673 };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "monitorings", type: ExchangeType.Fanout);
    }

    public void SendMessage(MonitoringDto message)
    {
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "monitorings", "", null, body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
