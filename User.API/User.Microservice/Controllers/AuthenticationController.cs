using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Data.Object.Helpers.DTO.Authentication;
using User.Services.Contracts;

namespace User.Microservice.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequestDto authenticationRequestDto)
    {
        var response = await _authenticationService.LoginAsync(authenticationRequestDto);
        return Ok(response);
    }
}
