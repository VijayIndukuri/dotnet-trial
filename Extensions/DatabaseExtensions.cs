using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using dotnet_trial.Models;

namespace dotnet_trial.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Registers <see cref="TodoContext"/> using a MySQL provider.
    /// Connection string is built from DB_* environment variables, or falls back to
    /// ConnectionStrings:DefaultConnection from configuration.
    /// </summary>
    public static IServiceCollection AddMySqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = BuildConnectionString(configuration);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "MySQL connection string not found. Ensure DB_* env vars are set or ConnectionStrings:DefaultConnection is provided.");
        }

        services.AddDbContext<TodoContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        return services;
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var host = Environment.GetEnvironmentVariable("DB_HOST_ADDRESS");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var db   = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER_NAME");
        var pwd  = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(db) || string.IsNullOrWhiteSpace(user))
        {
            return configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        port ??= "3306";
        return $"server={host};port={port};database={db};user={user};password={pwd}";
    }
} 