using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Data.Objects.Entities;
[Table("Messages")]
public class MessageEntity
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Admin is required!")]
    public Guid AdminId { get; set; }

    [Required(ErrorMessage = "User is required!")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Message is required!")]
    [MaxLength(500, ErrorMessage = "Message can't be longer than 500 characters!")]
    public string Message { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsAdminSeen { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsUserSeen { get; set; }

    [Required]
    public DateTime SentAt { get; set; }
}
