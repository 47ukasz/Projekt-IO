using projekt_io.DTOs;

namespace projekt_io.Models;

public class ProfileViewModel {
    public string CurrentTab { get; set; }
    public string UserFullName { get; set; }
    public string UserEmail { get; set; }
    public List<LostReportDto> Reports { get; set; }
}