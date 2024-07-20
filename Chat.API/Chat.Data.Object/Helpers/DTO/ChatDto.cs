namespace Chat.Data.Objects.Helpers.DTO;
public class ChatDto
{
    public string Username { get; set; }
    public ICollection<MessageDto> Messages { get; set; }
}
