using User.Data.Access;
using User.Data.Contracts;
using User.Services.Business;
using User.Services.Contracts;

namespace User.Microservice.Infrastructure;

public static class ServiceExtensions {
    public static IServiceCollection RegisterServices(this IServiceCollection services) {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddAutoMapper(typeof(Data.Contracts.Helpers.Mapper));

        services.AddHttpClient<ICommunicationService, CommunicationService>(client => {
            client.BaseAddress = new Uri("http://device-microservice");
        });
        //http://device-microservice
        //http://localhost:5121
        services.AddCors(options => options.AddPolicy(
            name: "NgOrigins",
            policy => {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://device-microservice").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://energy-management-frontend").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.WithOrigins("http://localhost:5121").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        return services;
    }
}
