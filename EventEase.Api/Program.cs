using EventEase.Api.Data;
using EventEase.Api.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<EventEaseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventEaseDb")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7002",
                "http://localhost:5227",
                "https://localhost:7257",
                "http://localhost:5100")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.MapControllers();
app.MapHub<AttendanceHub>("/hubs/attendance");

app.Run();
