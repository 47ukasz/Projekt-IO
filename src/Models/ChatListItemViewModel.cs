namespace projekt_io.Models;

public class ChatListItemViewModel
{
    public string ChatId { get; set; }
    public string OtherUserDisplay { get; set; }
    public string LastMessagePreview { get; set; }
    public DateTime? LastMessageAt { get; set; }
}
