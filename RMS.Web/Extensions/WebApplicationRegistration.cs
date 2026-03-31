using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Persistence.Data.Contexts;

namespace RMS.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var pendingMaigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMaigrations.Any())
                await dbContextService.Database.MigrateAsync();
            return app;
        }

        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataInitializerService.InitializeAsync();
            return app;
        } 
    }
}
