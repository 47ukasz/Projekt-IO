using projekt_io.DTOs;
using projekt_io.Entities;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class AnimalMapper {
    public static partial AnimalDto ToDto(Animal dto);
    public static partial Animal ToEntity(AnimalDto dto);
}