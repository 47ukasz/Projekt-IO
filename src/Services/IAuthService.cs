using projekt_io.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using projekt_io.Entities;
using projekt_io.Results;

namespace projekt_io.Services;

public interface IAuthService {
    public Task<bool> RegisterAsync(RegisterDto registerDto);
    public Task<bool> LoginAsync(LoginDto loginDto);
    public Task LogoutAsync();
    public Task<ActivateAccountResult> ActivateAccountAsync(string userId, string token);
    public Task<(ApplicationUser, string)> GenerateConfirmationToken(string email);
}
