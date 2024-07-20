using Chat.Data.Objects.Entities;

namespace Chat.Data.Contracts;
public interface IMessageRepository
{
    public Task AddMessageAsync(MessageEntity message);
    public Task<ICollection<MessageEntity>> GetMessagesByAdminIdAndUsernamesAsync(Guid adminId, ICollection<string> username);
    public Task<ICollection<MessageEntity>> GetMessagesByAdminIdAndUsernameAsync(Guid adminId, string username);
    public Task UpdateMessagesAsync(ICollection<MessageEntity> messages);
}
