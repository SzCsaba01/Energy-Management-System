namespace Chat.Data.Objects.Helpers.DTO;
public class TypeingDto
{
    public Guid AdminId { get; set; }
    public string Username { get; set; }
    public bool IsAdminTyping { get; set; }
    public bool IsUserTyping { get; set; }
}
