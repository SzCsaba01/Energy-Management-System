using Chat.Data.Objects.Helpers.DTO;
using Chat.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ITypeingService _typeingService;

    public MessageController(IMessageService messageService, ITypeingService typeingService)
    {
        _messageService = messageService;
        _typeingService = typeingService;
    }

    [HttpPost("SendMessage")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> SendMessageAsync([FromBody] MessageDto message)
    {
        await _messageService.SendMessageAsync(message);

        return Ok();
    }

    [HttpPost("GetMessagesByAdminIdAndUsernames")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetMessagesByAdminIdAndUsernamesAsync([FromBody] ICollection<string> usernames, [FromQuery] Guid adminId)
    {
        var chats = await _messageService.GetMessagesByAdminIdAndUsernamesAsync(adminId, usernames);

        return Ok(chats);
    }

    [HttpGet("GetMessagesByAdminIdAndUsername")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetMessagesByAdminIdAndUsernameAsync([FromQuery] Guid adminId, [FromQuery] string username)
    {
        var messages = await _messageService.GetMessagesByAdminIdAndUsernameAsync(adminId, username);

        return Ok(messages);
    }

    [HttpPut("UpdateMessage")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> UpdateMessageAsync([FromBody] ICollection<MessageDto> message)
    {
        await _messageService.UpdateMessagesAsync(message);

        return Ok();
    }

    [HttpPut("Typing")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> TypingAsync([FromBody] TypeingDto typeing)
    {
        await _typeingService.TypingAsync(typeing);

        return Ok();
    }
}
