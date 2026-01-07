using projekt_io.DTOs;

namespace projekt_io.Models;

public class LostReportPageViewModel {
    public LostReportDto? LostReport { get; set; }
    public List<SightingsCommentViewModel> SightingsComments { get; set; }
}