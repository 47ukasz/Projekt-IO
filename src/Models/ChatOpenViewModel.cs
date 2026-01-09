namespace projekt_io.Models;

public class ChatOpenViewModel
{
    public string ChatId { get; set; }
    public string OtherUserDisplay { get; set; }
    public List<ChatMessageViewModel> Messages { get; set; } = new();
}
