namespace projekt_io.Models;

public class ProfileViewModel
{
    public string CurrentTab { get; set; } = "reports";

    public List<ChatListItemViewModel> Chats { get; set; } = new();
}
