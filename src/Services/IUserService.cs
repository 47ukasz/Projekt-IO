using projekt_io.DTOs;
using projekt_io.Entities;

namespace projekt_io.Services;

public interface IUserService {
    public Task<List<UserDto>> GetAllUsersAsync();
    public Task<bool> BlockUserAsync(string userId);
    public Task<bool> UnblockUserAsync(string userId);
    public Task<UserDto> GetUserById(string userId);
}