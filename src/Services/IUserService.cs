using projekt_io.Entities;

namespace projekt_io.Services;

public interface IUserService {
    public Task<ApplicationUser> GetUserById(string userId);
}