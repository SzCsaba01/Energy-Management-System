using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringAndCommunication.Data.Object.Helpers.DTO.Monitoring;
using MonitoringAndCommunication.Services.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MonitoringAndCommunication.Services.Business;
public class MonitoringMessageQueueBackgroundService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string queueName;
    private readonly IServiceProvider _serviceProvider;
    public MonitoringMessageQueueBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        //HostName = "localhost",
        //HostName = monitoring-queue
        var factory = new ConnectionFactory() { 
            HostName = "monitoring-queue", 
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "monitorings", type: ExchangeType.Fanout);
        queueName = _channel.QueueDeclare().QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        _channel.QueueBind(queue: queueName, exchange: "monitorings", routingKey: "");

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
        var _monitoringService = scope.ServiceProvider.GetRequiredService<IMonitoringService>();

        var messageObject = JsonConvert.DeserializeObject<MonitoringDto>(message);

        if (messageObject is null)
        {
            throw new Exception("Message is null");
        }

        await _monitoringService.AddMonitoring(messageObject);
    }
}
