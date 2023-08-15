using Microsoft.EntityFrameworkCore.SqlServer;
using MarvelTest.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using MarvelTest.Data;
using Microsoft.EntityFrameworkCore;
using MarvelTest.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<MarvelDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<MarvelDBContext>();
        var seeder = new DatabaseSeeder(context, new MarvelService(new HttpClient())); 
        seeder.SeedDatabaseAsync().Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al llenar la base de datos: {ex.Message}");
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();