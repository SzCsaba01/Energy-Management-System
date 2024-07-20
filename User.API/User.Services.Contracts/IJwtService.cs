using Microsoft.Extensions.Configuration;
using Project.Data.Data.Entities;

namespace User.Services.Contracts;
public interface IJwtService
{
    Task<string> GetAuthentificationJwtAsync(UserEntity user);
}
