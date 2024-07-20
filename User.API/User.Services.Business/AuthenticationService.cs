using Project.Data.Data;
using System.Security.Authentication;
using User.Data.Contracts;
using User.Data.Object.Helpers.DTO.Authentication;
using User.Services.Contracts;

namespace User.Services.Business;
public class AuthenticationService: IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtService _jwtService;
    public AuthenticationService(IUserRepository userRepository, IEncryptionService encryptionService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _jwtService = jwtService;
    }
    public async Task<AuthenticationResponseDto> LoginAsync(AuthenticationRequestDto authenticationRequestDto)
    {
        authenticationRequestDto.Password = _encryptionService.GeneratedHashedPassword(authenticationRequestDto.Password);

        var user = await _userRepository.GetUserByUsernameAsync(authenticationRequestDto.UserCredentials);
        
        if (user is null)
        {
            throw new Exception("Username or Password is incorrect");
        }

        var token = await _jwtService.GetAuthentificationJwtAsync(user);
        return new AuthenticationResponseDto { Id = user.Id, Role = user.Role, Token = token, Username = user.Username };
    }
}
