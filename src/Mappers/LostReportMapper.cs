using projekt_io.DTOs;
using projekt_io.Entities;
using Riok.Mapperly.Abstractions;

namespace projekt_io.Mappers;

[Mapper]
public partial class LostReportMapper {
     public static partial LostReportDto ToDto(LostReport dto);
     [MapperIgnoreTarget(nameof(Sighting.Id))]
     public static partial LostReport ToEntity(LostReportDto dto);
}