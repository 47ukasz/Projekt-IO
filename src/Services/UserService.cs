using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Mappers;

namespace projekt_io.Services;

public class UserService : IUserService {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public UserService(UserManager<ApplicationUser> userManager, AppDbContext context) {
        _userManager = userManager;
        _context = context;
    }

    public async Task<List<UserDto>> GetAllUsersAsync() {
        var users = await _context.Users.ToListAsync();
        
        var convertedUsers = new List<UserDto>();

        foreach (var user in users) {
            var userDto = UserMapper.ToDto(user);
            var roles = await _userManager.GetRolesAsync(user);
            
            userDto.Roles = roles.ToList();
            
            convertedUsers.Add(userDto);
        }
        
        return convertedUsers;
    }

    
    public async Task<bool> BlockUserAsync(string userId) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) {
            return false;
        }

        if (user.Blocked) {
            return true;
        }
        
        user.Blocked = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnblockUserAsync(string userId) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) {
            return false;
        }

        if (!user.Blocked) {
            return true;
        }
        
        user.Blocked = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserDto> GetUserById(string userId) {
        if (string.IsNullOrEmpty(userId)) {
            return null;
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        
        var userDto = UserMapper.ToDto(user);
        var roles = await _userManager.GetRolesAsync(user);
        userDto.Roles = roles.ToList();
        
        return userDto;
    }

}