using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Message;
using MonitoringAndCommunication.Services.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MonitoringAndCommunication.Services.Business;
public class DeviceMessageQueueBackgroundService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string queueName;
    private readonly IServiceProvider _serviceProvider;
    public DeviceMessageQueueBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        //HostName = "localhost",
        //HostName = "device-queue"
        var factory = new ConnectionFactory() { 
            HostName = "device-queue", 
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "devices", type: ExchangeType.Fanout);
        queueName = _channel.QueueDeclare().QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        _channel.QueueBind(queue: queueName, exchange: "devices", routingKey: "");

        consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await ProcessMessageAsync(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(100, stoppingToken); 
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _connection.Close();
        await base.StopAsync(cancellationToken);
    }

    private async Task ProcessMessageAsync(string message)
    {
        using var scope = _serviceProvider.CreateScope();
        var _deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

        var messageObject = JsonConvert.DeserializeObject<DeviceMessageDto>(message);
        
        if (messageObject is null)
        {
            throw new Exception("Message is null");
        }

        switch (messageObject.MessageType) 
        {
            case "Add":
                await _deviceService.AddDeviceAsync(messageObject.Device);
                break;
            case "Delete":
                await _deviceService.RemoveDeviceByIdAsync(messageObject.Device.DeviceId);
                break;
            case "Update":
                await _deviceService.UpdateDeviceAsync(messageObject.Device);
                break;
            default:
                throw new Exception("Message type is not supported");
        }
    }
}
