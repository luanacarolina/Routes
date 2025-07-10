using Route.Domain.Entities;

namespace Route.Domain.Contracts.Repositories;

public interface IRouteRepository
{
    Task<IEnumerable<Routes>> GetAllAsync();
    Task<Routes?> GetByIdAsync(int id);
    Task AddAsync(Routes route);
    Task UpdateAsync(Routes route);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
