using System.Net;
using System.Net.Mail;

namespace projekt_io.Services;

public class EmailService : IEmailService {
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) {
        _config = config;
    }
    
    public async Task SendEmailAsync(string email, string subject, string message) {
        var address = _config["Email:Address"];
        var password = _config["Email:Password"];
        
        var client = new SmtpClient(_config["Email:Host"], int.Parse(_config["Email:Port"])) {
            Credentials = new NetworkCredential(address, password),
            EnableSsl = true
        };

        var mail = new MailMessage() {
            From = new MailAddress(address),
            To = { new MailAddress(email) },
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        
        mail.To.Add(address);
        
        await client.SendMailAsync(mail);
    }
}