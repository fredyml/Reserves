using Microsoft.EntityFrameworkCore;
using Reserves.Application.Interfaces;
using Reserves.Application.Services;
using Reserves.Infrastructure.Data;
using Reserves.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuraci�n de Entity Framework Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyecci�n de dependencias para repositorios gen�ricos
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Inyecci�n de dependencias para servicios
builder.Services.AddScoped<ReservationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();  

app.Run();
