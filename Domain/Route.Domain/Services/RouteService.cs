using Route.Domain.Contracts.Repositories;
using Route.Domain.Contracts.Services;
using Route.Domain.Entities;

namespace Route.Domain.Services;

public class RouteService(IRouteRepository repository) : IRouteService
{
    private readonly IRouteRepository _repository = repository;

    public async Task<IEnumerable<Routes>> GetAllRoutesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Routes?> GetRouteByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Routes> CreateRouteAsync(Routes route)
    {
        await _repository.AddAsync(route);
        await _repository.SaveChangesAsync();
        return route;
    }
    public async Task<string> CalculateBestRoute(string origin, string destination)
    {
        var routes = await _repository.GetAllAsync();

        var graph = new Dictionary<string, List<(string dest, decimal cost)>>();

        foreach (var route in routes)
        {
            if (!graph.ContainsKey(route.Origin))
                graph[route.Origin] = [];
            graph[route.Origin].Add((route.Destination, route.Price));
        }

        List<string> bestPath = null!;
        decimal bestCost = decimal.MaxValue;

        void Search(string current, List<string> path, decimal cost)
        {
            if (cost > bestCost) return;
            if (current == destination)
            {
                bestCost = cost;
                bestPath = new List<string>(path);
                return;
            }

            if (!graph.ContainsKey(current)) return;

            foreach (var (next, nextCost) in graph[current])
            {
                if (path.Contains(next)) continue;

                path.Add(next);
                Search(next, path, cost + nextCost);
                path.RemoveAt(path.Count - 1);
            }
        }

        Search(origin, new List<string> { origin }, 0);

        if (bestPath == null)
            return "Route not found.";

        return $"{string.Join(" - ", bestPath)} ao custo de ${bestCost}";
    }


    public async Task<bool> UpdateRouteAsync(Routes route)
    {
        var existing = await _repository.GetByIdAsync(route.Id);
        if (existing == null)
            return false;

        existing.Origin = route.Origin;
        existing.Destination = route.Destination;
        existing.Price = route.Price;

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRouteAsync(int id)
    {
        var route = await _repository.GetByIdAsync(id);
        if (route == null)
            return false;

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return true;
    }
}
