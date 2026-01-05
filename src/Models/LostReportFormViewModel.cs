using System.ComponentModel.DataAnnotations;

namespace projekt_io.Models;

public class LostReportFormViewModel {
    public bool IsEdit { get; set; }
    public string? Id { get; set; }

    [Required(ErrorMessage = "Podaj imię zwierzaka.")]
    [StringLength(50)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Wybierz gatunek.")]
    public string Species { get; set; } 
    
    public string? Breed { get; set; }
    
    [Required(ErrorMessage = "Podaj tytuł zgłoszenia.")]
    [StringLength(50)]
    public string Title { get; set; } = "";
    
    [Required(ErrorMessage = "Podaj opis zgłoszenia.")]
    [StringLength(1000)]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Podaj datę zaginięcia.")]
    public DateOnly? MissingDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Wybierz lokalizację na mapie.")]
    public string? Lat { get; set; }

    public string? Lng { get; set; }

    public IFormFile? Photo { get; set; }
}