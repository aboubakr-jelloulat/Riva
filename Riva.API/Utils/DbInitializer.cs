using Microsoft.EntityFrameworkCore;
using Riva.API.Data.Context;

namespace Riva.API.Utils;

// It automatically updates the database when the application starts.
public static class DbInitializer
{
    public static async Task SeedDataAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }
}