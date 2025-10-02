using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookCatalog.Infrastructure.Data.Extentions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedAuthorAsync(context);
        await SeedGenreAsync(context);
        await SeedBookAsync(context);
    }

    private static async Task SeedAuthorAsync(ApplicationDbContext context)
    {
        if (!await context.Authors.AnyAsync())
        {
            await context.Authors.AddRangeAsync(InitialData.Authors);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedBookAsync(ApplicationDbContext context)
    {
        if (!await context.Books.AnyAsync())
        {
            await context.Books.AddRangeAsync(InitialData.Books);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedGenreAsync(ApplicationDbContext context)
    {
        if (!await context.Genres.AnyAsync())
        {
            await context.Genres.AddRangeAsync(InitialData.Genres);
            await context.SaveChangesAsync();
        }
    }
}