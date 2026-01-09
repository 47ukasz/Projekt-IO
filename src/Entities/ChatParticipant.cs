namespace projekt_io.Entities;

public class ChatParticipant
{
    public string ChatId { get; set; }
    public Chat Chat { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
