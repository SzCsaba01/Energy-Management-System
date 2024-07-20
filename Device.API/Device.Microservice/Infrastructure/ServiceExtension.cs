using Device.Data.Access;
using Device.Data.Contracts;
using Device.Services.Business;
using Device.Services.Contracts;

namespace Device.Microservice.Infrastructure;

public static class ServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddAutoMapper(typeof(User.Data.Contracts.Helpers.Mapper));
        services.AddScoped<IDeviceMessageQueueService, DeviceMessageQueueService>();

        services.AddHttpClient<ICommunicationService, CommunicationService>(client => {
            client.BaseAddress = new Uri("http://user-microservice");
        });
        //http://user-microservice
        //http://localhost:5076
        services.AddCors(options => options.AddPolicy(
            name: "NgOrigins",
            policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://energy-management-frontend").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://user-microservice").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://localhost:5076").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        return services;
    }
}
