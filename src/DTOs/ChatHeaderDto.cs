namespace projekt_io.DTOs;

public class ChatHeaderDto {
    public string ChatId { get; set; }
    public string TargetTitle { get; set; }
    
    public UserShortDto Creator { get; set; }
    public UserShortDto Owner { get; set; }
    
    public DateTime CreatedAt { get; set; }
}