using Microsoft.EntityFrameworkCore;
using Route.Domain.Contracts.Repositories;
using Route.Domain.Entities;
using Route.Infra.Data;

namespace Route.Infra.Repositories;

public class RouteRepository(RoutesContext context) : IRouteRepository
{
    private readonly RoutesContext _context = context;

    public async Task<IEnumerable<Routes>> GetAllAsync() =>
        await _context.Routes.ToListAsync();

    public async Task<Routes?> GetByIdAsync(int id) =>
        await _context.Routes.FindAsync(id);

    public async Task AddAsync(Routes route) =>
        await _context.Routes.AddAsync(route);

    public Task UpdateAsync(Routes route)
    {
        _context.Routes.Update(route);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var route = await GetByIdAsync(id);
        if (route != null)
            _context.Routes.Remove(route);
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
