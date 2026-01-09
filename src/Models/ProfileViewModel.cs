using projekt_io.DTOs;

namespace projekt_io.Models;

public class ProfileViewModel {
    public bool IsAdminView { get; set; }
    public string CurrentUserId { get; set; }
    public string TargetUserId { get; set; }
    public string CurrentTab { get; set; }
    public string UserFullName { get; set; }
    public string UserEmail { get; set; }
    public List<LostReportDto> Reports { get; set; }
    public List<SightingDto> Sightings { get; set; }
    public List<UserDto> Users { get; set; }
    public List<ChatListItemViewModel> Chats { get; set; } = new();
}