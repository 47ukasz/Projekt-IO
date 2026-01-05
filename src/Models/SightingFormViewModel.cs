using System.ComponentModel.DataAnnotations;

namespace projekt_io.Models;

public class SightingFormViewModel {
    public bool IsEdit { get; set; }
    public string? Id { get; set; }
    public string LostReportId { get; set; }
    
    [Required(ErrorMessage = "Podaj opis zgłoszenia.")]
    [StringLength(1000)]
    public string Description { get; set; } = "";
    
    [Required(ErrorMessage = "Podaj datę obserwacji.")]
    public DateOnly SeenDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    
    [Required(ErrorMessage = "Wybierz lokalizację na mapie.")]
    public string? Lat { get; set; }

    public string? Lng { get; set; }
    
    [Required(ErrorMessage = "Podaj zdjęcie zauwarzonego zwierzaka.")]
    public IFormFile Photo { get; set; }
}