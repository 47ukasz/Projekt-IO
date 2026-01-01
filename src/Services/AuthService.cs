using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using projekt_io.Data;
using projekt_io.DTOs.Auth;
using projekt_io.Entities;

namespace projekt_io.Services;

public class AuthService : IAuthService {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
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

    public bool ActivateAccountAsync(string token) {
        throw new NotImplementedException();
    }
}