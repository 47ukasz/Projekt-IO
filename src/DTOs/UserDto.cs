namespace projekt_io.DTOs;

public class UserDto {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string>? Roles { get; set; }
    public bool Blocked { get; set; }
}