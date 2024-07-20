using Chat.Data.Access;
using Chat.Data.Contracts;
using Chat.Data.Objects.Helpers;
using Chat.Services.Business;
using Chat.Services.Contracts;

namespace Chat.API.Infrastructure;

public static class ServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ITypeingService, TypeingService>();
        services.AddAutoMapper(typeof(Mapper));

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
