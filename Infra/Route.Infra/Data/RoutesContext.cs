using Microsoft.EntityFrameworkCore;
using Route.Domain.Entities;

namespace Route.Infra.Data;

public class RoutesContext(DbContextOptions<RoutesContext> options) : DbContext(options)
{
    public DbSet<Routes> Routes => Set<Routes>();
}
