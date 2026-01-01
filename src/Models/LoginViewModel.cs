using System.ComponentModel.DataAnnotations;

namespace projekt_io.Models;

public class LoginViewModel {
    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres email.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(6, ErrorMessage = "Hasło musi składać się z conajmniej 6 znaków.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}