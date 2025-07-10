using Route.Domain.Entities;

namespace Route.Domain.Contracts.Services;

public interface IRouteService
{
    Task<IEnumerable<Routes>> GetAllRoutesAsync();
    Task<Routes?> GetRouteByIdAsync(int id);
    Task<string> CalculateBestRoute(string origin, string destination);

    Task<Routes> CreateRouteAsync(Routes route);
    Task<bool> UpdateRouteAsync(Routes route);
    Task<bool> DeleteRouteAsync(int id);
}
