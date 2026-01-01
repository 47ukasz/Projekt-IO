using projekt_io.DTOs.Auth;
using projekt_io.Models;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class UserMapper {
    public partial RegisterDto ToDto(RegisterViewModel model);
}