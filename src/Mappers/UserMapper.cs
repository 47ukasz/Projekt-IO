using projekt_io.DTOs;
using projekt_io.DTOs.Auth;
using projekt_io.Entities;
using projekt_io.Models;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class UserMapper {
    public static partial UserDto ToDto(ApplicationUser user);
    public static partial ApplicationUser ToEntity(UserDto dto);
}