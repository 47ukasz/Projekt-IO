using System.ComponentModel.DataAnnotations;

namespace projekt_io.Models;

public class RegisterViewModel {
    [Required(ErrorMessage = "Imię użytkownika jest wymagane.")]
    [MinLength(2, ErrorMessage = "Imię użytkownika musi składać się z conajmniej dwóch znaków.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Nazwisko użytkownika jest wymagane.")]
    [MinLength(2, ErrorMessage = "Nazwisko użytkownika musi składać się z conajmniej 2 znaków.")]
    public string? SecondName { get; set; }
    
    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres email.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(6, ErrorMessage = "Hasło musi składać się z conajmniej 6 znaków.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Potwierdź hasło.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Hasła muszą być takie same.")]
    public string ConfirmPassword { get; set; }

}