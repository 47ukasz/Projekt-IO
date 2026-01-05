using projekt_io.DTOs;
using projekt_io.Entities;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class SightingMapper {
    public static partial SightingDto ToDto(Sighting sighting);
    [MapperIgnoreTarget(nameof(Sighting.Id))]
    public static partial Sighting ToEntity(SightingDto dto);
}