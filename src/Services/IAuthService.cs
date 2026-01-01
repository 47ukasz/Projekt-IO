using projekt_io.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
namespace projekt_io.Services;

public interface IAuthService {
    public Task<bool> RegisterAsync(RegisterDto registerDto);
    public Task<bool> LoginAsync(LoginDto loginDto);
    public bool LogoutAsync();
    public bool ActivateAccountAsync(string token);
}