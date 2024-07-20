using Chat.Data.Objects.Helpers.DTO;
using Microsoft.Identity.Client;

namespace Chat.Services.Contracts;
public interface ITypeingService
{
    public Task TypingAsync(TypeingDto typeing);
}
