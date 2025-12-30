using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.Middleware;
using VideoGameCharacterApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --- DI registrations ---
builder.Services.AddHttpContextAccessor();                   // required for RequestContext
builder.Services.AddScoped<IRequestContext, RequestContext>(); // one per request

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

// Custom middleware
Console.WriteLine();
Console.WriteLine("Registering custom middleware");
app.UseMiddleware<HelloMiddleware>();
app.UseMiddleware<RequestContextMiddleware>();
Console.WriteLine();

app.MapControllers();

app.Run();
