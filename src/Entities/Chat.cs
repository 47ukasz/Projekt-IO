namespace projekt_io.Entities;

public class Chat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ChatParticipant> Participants { get; set; } = new();
    public List<Message> Messages { get; set; } = new();
}
