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
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager) {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
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

    public async Task<bool> ChangeUserRoles(string userId, string currentUserId, List<string> roles) {
        if (string.IsNullOrEmpty(userId) || roles == null || roles.Count == 0) {
            return false;
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        var isCurrentUserChanging = userId == currentUserId;
        
        if (user == null) {
            return false;
        }
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        var rolesToAdd = roles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(roles).ToList();

        if (isCurrentUserChanging && currentRoles.Contains("Admin") && !roles.Contains("Admin")) {
            return false;
        }
        
        if (rolesToAdd.Count > 0) {
            await _userManager.AddToRolesAsync(user, rolesToAdd);
        }

        if (rolesToRemove.Count > 0) {
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        return true;
    }
}