namespace projekt_io.Entities;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ChatId { get; set; }
    public Chat Chat { get; set; }

    public string SenderId { get; set; }
    public ApplicationUser Sender { get; set; }

    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
