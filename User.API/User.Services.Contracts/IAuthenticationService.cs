using User.Data.Object.Helpers.DTO.Authentication;

namespace User.Services.Contracts;
public interface IAuthenticationService
{
    Task<AuthenticationResponseDto> LoginAsync(AuthenticationRequestDto authenticationRequestDto);
}
