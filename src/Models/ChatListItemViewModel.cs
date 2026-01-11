namespace projekt_io.Models;

public class ChatListItemViewModel
{
    public string ChatId { get; set; }
    public string ReceiverId { get; set; }
    public string ReceiverFirstName { get; set; }
    public string ReceiverLastName { get; set; }
    public string LastMessagePreview { get; set; }
    public DateTime? LastMessageAt { get; set; }
}
