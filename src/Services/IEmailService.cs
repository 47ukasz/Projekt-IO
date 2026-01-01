using projekt_io.Entities;

namespace projekt_io.Services;

public interface IEmailService {
    public void SendConfirmationEmail(ApplicationUser user, string url);
}