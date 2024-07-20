namespace User.Data.Object.Helpers.DTO.Authentication;
public class AuthenticationResponseDto
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public string Username { get; set; }
}
