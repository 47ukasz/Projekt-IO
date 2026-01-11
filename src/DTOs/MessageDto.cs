namespace projekt_io.DTOs;

public class MessageDto {
    public string SenderId { get; set; }
    public string Content {get; set;}
    public bool IsMine {get; set;}
    public DateTime SentAt {get; set;}
}