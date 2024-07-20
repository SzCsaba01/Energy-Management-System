using Chat.Data.Objects.Helpers.DTO;

namespace Chat.Services.Contracts;
public interface IMessageService
{
    public Task SendMessageAsync(MessageDto message);
    public Task<ICollection<ChatDto>> GetMessagesByAdminIdAndUsernamesAsync(Guid adminId, ICollection<string> username);
    public Task<ChatDto> GetMessagesByAdminIdAndUsernameAsync(Guid adminId, string username);
    public Task UpdateMessagesAsync(ICollection<MessageDto> message);
    public Task JoinGroupAsync(Guid adminId, string username);
    public Task LeaveGroupAsync(Guid adminId, string username);
}
