using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVideoGameCharacterService, VideoGameCharacterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // use scalar UI to explore the API
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.Use(async (context, next) =>
{
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Hello from middleware!");
    await next();
});

app.MapControllers();

app.Run();
