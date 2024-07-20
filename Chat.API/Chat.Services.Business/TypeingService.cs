using Chat.Data.Objects.Helpers.DTO;
using Chat.Services.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Services.Business;
public class TypeingService : ITypeingService
{
    private readonly IHubContext<MessageService> _hubContext;

    public TypeingService(IHubContext<MessageService> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task TypingAsync(TypeingDto typeing)
    {
        await _hubContext.Clients.Group(typeing.AdminId.ToString() + typeing.Username).SendAsync("ReceiveTyping", typeing);
    }
}
