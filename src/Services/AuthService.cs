using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using projekt_io.Controllers;
using projekt_io.Data;
using projekt_io.DTOs.Auth;
using projekt_io.Entities;
using projekt_io.Results;

namespace projekt_io.Services;

public class AuthService : IAuthService {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger) {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<bool> RegisterAsync(RegisterDto registerDto) {
        if (registerDto == null) {
            return false;
        }
        
        var exists = await _userManager.FindByEmailAsync(registerDto.Email);

        if (exists != null) {
            return false;
        }

        var user = new ApplicationUser {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) {
            return false;
        }
        
        return true;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto) {
        if (loginDto == null) {
            return false;
        }

        var result = await _signInManager.PasswordSignInAsync(userName: loginDto.Email,
            password: loginDto.Password, false, false);

        if (!result.Succeeded) {
            return false;
        }

        return true;
    }

    public Task LogoutAsync() {
        return _signInManager.SignOutAsync();
    }

    public async Task<ActivateAccountResult> ActivateAccountAsync(string userId, string token) {
        if (userId == null || token == null) {
            return ActivateAccountResult.InvalidOrExpired;
        }
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) {
            return ActivateAccountResult.InvalidOrExpired;
        }

        if (user.EmailConfirmed) {
            return ActivateAccountResult.AlreadyActivated;
        }
        
        string tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        if (tokenDecoded == null) {
            return ActivateAccountResult.InvalidOrExpired;
        }
        
        var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

        if (!result.Succeeded) {
            return ActivateAccountResult.InvalidOrExpired;
        }
        
        return ActivateAccountResult.Success;
    }

    public async Task<(ApplicationUser, string)> GenerateConfirmationToken(string email) {
        if (email == null) {
            return (null, String.Empty);
        }
        
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) {
            return (null, String.Empty);
        }
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        return (user, tokenEncoded);
    }

} 