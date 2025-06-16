using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using dotnet_trial.Extensions;
using dotnet_trial.Models;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();     // if you want to use MVC controllers
builder.Services.AddAuthorization();  
builder.Services.AddSwaggerGen();

// Register MySQL DbContext via extension method
builder.Services.AddMySqlDatabase(builder.Configuration);

var app = builder.Build();

// Automatically run EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();