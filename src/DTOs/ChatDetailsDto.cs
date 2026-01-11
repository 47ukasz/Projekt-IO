namespace projekt_io.DTOs;

public class ChatDetailsDto {
    public ChatHeaderDto Chat { get; set; }
    public List<MessageDto> Messages { get; set; }
}