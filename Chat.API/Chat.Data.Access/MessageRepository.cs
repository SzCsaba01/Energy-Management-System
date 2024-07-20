using Chat.Data.Contracts;
using Chat.Data.Objects.Data;
using Chat.Data.Objects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Access;
public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddMessageAsync(MessageEntity message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<MessageEntity>> GetMessagesByAdminIdAndUsernamesAsync(Guid adminId, ICollection<string> usernames)
    {
        return await _context.Messages
            .Where(m => m.AdminId == adminId && usernames.Contains(m.Username))
            .OrderByDescending(m => m.SentAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<MessageEntity>> GetMessagesByAdminIdAndUsernameAsync(Guid adminId, string username)
    {
        return await _context.Messages
            .Where(m => m.AdminId == adminId && m.Username == username)
            .OrderByDescending(m => m.SentAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateMessagesAsync(ICollection<MessageEntity> messages)
    {
        _context.Messages.UpdateRange(messages);
        await _context.SaveChangesAsync();
    }
}
