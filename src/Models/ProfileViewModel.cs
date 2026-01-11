using projekt_io.DTOs;

namespace projekt_io.Models;

public class ProfileViewModel {
    public bool IsAdminView { get; set; }
    public string CurrentUserId { get; set; }
    public string TargetUserId { get; set; }
    public string CurrentTab { get; set; }
    public string UserFullName { get; set; }
    public string UserEmail { get; set; }
    public ListViewModel<LostReportDto> Reports { get; set; }
    public List<SightingDto> Sightings { get; set; } = new List<SightingDto>();
    public List<UserDto> Users { get; set; } = new List<UserDto>();
    public List<ChatListItemViewModel> Chats { get; set; } = new List<ChatListItemViewModel>();
}