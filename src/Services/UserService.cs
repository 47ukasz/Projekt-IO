using Microsoft.AspNetCore.Identity;
using projekt_io.Entities;

namespace projekt_io.Services;

public class UserService : IUserService {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager) {
        _userManager = userManager;
    }
    
    public Task<ApplicationUser> GetUserById(string userId) {
        if (string.IsNullOrEmpty(userId)) {
            return null;
        }
        
        return _userManager.FindByIdAsync(userId);
    }
}