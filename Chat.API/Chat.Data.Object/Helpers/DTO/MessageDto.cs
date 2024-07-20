namespace Chat.Data.Objects.Helpers.DTO;
public class MessageDto
{
    public Guid Id { get; set; }
    public Guid AdminId { get; set; }
    public string Username { get; set; }
    public string Message { get; set; }
    public bool IsAdminSeen { get; set; }
    public bool IsUserSeen { get; set; }
    public DateTime SentAt { get; set; }
}
