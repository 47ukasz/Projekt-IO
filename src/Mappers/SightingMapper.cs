using projekt_io.DTOs;
using projekt_io.Entities;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class SightingMapper {
    public static partial SightingDto ToDto(Sighting sighting);
    [MapperIgnoreTarget(nameof(Sighting.Id))]
    [MapperIgnoreTarget(nameof(Sighting.LostReport))]
    [MapperIgnoreTarget(nameof(Sighting.Location))]
    [MapperIgnoreTarget(nameof(Sighting.SeenDate))]
    public static partial Sighting ToEntity(SightingDto dto);
}