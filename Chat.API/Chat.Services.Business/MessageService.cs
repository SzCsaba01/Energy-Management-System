using AutoMapper;
using Chat.Data.Contracts;
using Chat.Data.Objects.Entities;
using Chat.Data.Objects.Helpers.DTO;
using Chat.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Chat.Services.Business;

public class MessageService : Hub, IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    private readonly IHubContext<MessageService> _hubContext;

    public MessageService(IMessageRepository messageRepository, IMapper mapper, IHubContext<MessageService> hubContext)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    public async Task SendMessageAsync(MessageDto message)
    {
        var messageEntity = _mapper.Map<MessageEntity>(message);

        messageEntity.SentAt = DateTime.Now;

        var result = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(messageEntity, new ValidationContext(messageEntity), result, true);
        var errorMessages = result.Select(r => r.ErrorMessage);

        if (!valid)
        {
            throw new ArgumentException($"The message is invalid: {string.Join(", ", errorMessages)}");
        }

        await _messageRepository.AddMessageAsync(messageEntity);

        var newMessage = _mapper.Map<MessageDto>(messageEntity);

        await _hubContext.Clients.Group(message.AdminId.ToString() + message.Username).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task<ICollection<ChatDto>> GetMessagesByAdminIdAndUsernamesAsync(Guid adminId, ICollection<string> usernames)
    {
        var messages = await _messageRepository.GetMessagesByAdminIdAndUsernamesAsync(adminId, usernames);

        var chats = messages
            .GroupBy(m => m.Username)
            .Select(g => new ChatDto
            {
                Username = g.Key,
                Messages = g.Select(m => _mapper.Map<MessageDto>(m)).OrderBy(m => m.SentAt).ToList()
            })
            .ToList();

        foreach(var username in usernames)
        {
            if (!chats.Any(c => c.Username == username))
            {
                chats.Add(new ChatDto
                {
                    Username = username,
                    Messages = new List<MessageDto>()
                });
            }
        }   

        return chats;
    }

    public async Task<ChatDto> GetMessagesByAdminIdAndUsernameAsync(Guid adminId, string username)
    {
        var messages = await _messageRepository.GetMessagesByAdminIdAndUsernameAsync(adminId, username);

        if (!messages.Any())
        {
            return new ChatDto
            {
                Username = "Admin",
                Messages = new List<MessageDto>()
            };
        }

        var chat = messages
            .GroupBy(m => m.Username)
            .Select(g => new ChatDto
            {
                Username = "Admin",
                Messages = g.Select(m => _mapper.Map<MessageDto>(m)).OrderBy(m => m.SentAt).ToList()
            })
            .FirstOrDefault();

        return chat;
    }

    public async Task UpdateMessagesAsync(ICollection<MessageDto> messages)
    {
        var messageEntities = _mapper.Map<ICollection<MessageEntity>>(messages);

        await _messageRepository.UpdateMessagesAsync(messageEntities);

        var newMessages = _mapper.Map<ICollection<MessageDto>>(messageEntities);

        // Assuming that AdminId and Username are properties of MessageDto.
        var groupKey = messages.First().AdminId.ToString() + messages.First().Username;

        await _hubContext.Clients.Group(groupKey).SendAsync("ReceiveUpdateMessage", newMessages);
    }

    public async Task JoinGroupAsync(Guid adminId, string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, adminId.ToString() + username);
    }

    public async Task LeaveGroupAsync(Guid adminId, string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, adminId.ToString() + username);
    }
}