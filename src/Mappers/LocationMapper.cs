using projekt_io.DTOs;
using projekt_io.Entities;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class LocationMapper {
    public static partial LocationDto ToDto(Location dto);
    public static partial Location ToEntity(LocationDto dto);
}