using projekt_io.Models;

namespace projekt_io.Services;

public interface IMapService {
    public Task<List<MapPointViewModel>> GetMapPointsAsync(MapFilterViewModel? filter = null);
}