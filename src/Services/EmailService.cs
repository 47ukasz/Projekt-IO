using System.Net;
using System.Net.Mail;
using projekt_io.Entities;

namespace projekt_io.Services;

public class EmailService : IEmailService {
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) {
        _config = config;
    }

    public async void SendConfirmationEmail(ApplicationUser user, string url) {
        await SendEmailAsync(
            user.Email!,
            "Potwierdzenie rejestracji",
            $"""
             <p>Witaj {user.FirstName},</p>
             <p>Aby aktywować konto, kliknij w link:</p>
             <p><a href="{url}">Aktywuj konto</a></p>
             <p>Jeżeli to nie Ty tworzyłeś konto — zignoruj tę wiadomość.</p>
             """
        );
    }
    
    private async Task SendEmailAsync(string email, string subject, string message) {
        var address = _config["Email:Address"];
        var password = _config["Email:Password"];
        
        var client = new SmtpClient(_config["Email:Host"], int.Parse(_config["Email:Port"])) {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(address, password),
            EnableSsl = true
        };

        var mail = new MailMessage() {
            From = new MailAddress(address),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        
        mail.To.Add(email);
        
        await client.SendMailAsync(mail);
    }
}