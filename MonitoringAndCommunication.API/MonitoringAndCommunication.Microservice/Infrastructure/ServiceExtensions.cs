using MonitoringAndCommunication.Data.Access;
using MonitoringAndCommunication.Data.Contracts;
using MonitoringAndCommunication.Data.Object.Helpers;
using MonitoringAndCommunication.Services.Business;
using MonitoringAndCommunication.Services.Contracts;

namespace MonitoringAndCommunication.Microservice.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IMonitoringRepository, MonitoringRepository>();
        services.AddScoped<IMonitoringService, MonitoringService>();
        services.AddAutoMapper(typeof(Mapper));
        services.AddHostedService<DeviceMessageQueueBackgroundService>();
        services.AddHostedService<MonitoringMessageQueueBackgroundService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddSignalR();

        services.AddCors(options => options.AddPolicy(
            name: "NgOrigins",
            policy =>
            {
                policy.WithOrigins("http://energy-management-frontend").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        return services;
    }
}
