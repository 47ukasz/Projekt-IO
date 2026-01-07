namespace projekt_io.Models;

public class ChatMessageViewModel
{
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsMine { get; set; }
}
