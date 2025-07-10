using Microsoft.EntityFrameworkCore;
using Route.Domain.Contracts.Repositories;
using Route.Domain.Contracts.Services;
using Route.Domain.Services;
using Route.Infra.Data;
using Route.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar serviços, incluindo controllers
builder.Services.AddControllers();

// Outros serviços, DI, DbContext, etc
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddDbContext<RoutesContext>(options =>
    options.UseSqlite("Data Source=rotas.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aqui: mapear as rotas dos controllers
app.MapControllers();

app.Run();
