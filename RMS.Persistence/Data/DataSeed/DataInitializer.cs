using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence.Data.Contexts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RMS.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly AppDbContext _dbContext;

        public DataInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var hasRoles = await _dbContext.Roles.AnyAsync();
                var hasBranches = await _dbContext.Branches.AnyAsync();
                var hasUsers = await _dbContext.Users.AnyAsync();
                var hasCategories = await _dbContext.Categories.AnyAsync();
                var hasIngredients = await _dbContext.Ingredients.AnyAsync();
                var hasMenuItems = await _dbContext.MenuItems.AnyAsync();
                var hasImages = await _dbContext.Images.AnyAsync();
                var hasRecipes = await _dbContext.Recipes.AnyAsync();
                var hasTables = await _dbContext.Tables.AnyAsync();
                var hasBranchStocks = await _dbContext.BranchStocks.AnyAsync();
                var hasOrders = await _dbContext.Orders.AnyAsync();
                var hasOrderItems = await _dbContext.OrderItems.AnyAsync();
                var hasKitchenTickets = await _dbContext.KitchenTickets.AnyAsync();
                var hasPayments = await _dbContext.Payments.AnyAsync();
                var hasDeliveries = await _dbContext.Deliveries.AnyAsync();
                var hasTableOrders = await _dbContext.TableOrders.AnyAsync();

                if (hasRoles && hasBranches && hasUsers && hasCategories && hasIngredients && hasMenuItems && hasImages &&
                    hasRecipes && hasTables && hasBranchStocks && hasOrders && hasOrderItems && hasKitchenTickets && hasPayments
                    && hasDeliveries && hasTableOrders) return;

                // ── 1. Roles (no dependencies) ────────────────────────────────────────────
                if (!hasRoles)
                {
                    await SeedDataFromJsonAsync<Role>("Roles.json", _dbContext.Roles);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 2. Branches (no dependencies) ─────────────────────────────────────────
                if (!hasBranches)
                {
                    await SeedDataFromJsonAsync<Branch>("Branches.json", _dbContext.Branches);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 3. Users (depends on: Roles, Branches) ────────────────────────────────
                if (!hasUsers)
                {
                    await SeedDataFromJsonAsync<User>("Users.json", _dbContext.Users);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 4. Categories (no dependencies) ───────────────────────────────────────
                if (!hasCategories)
                {
                    await SeedDataFromJsonAsync<Category>("Categories.json", _dbContext.Categories);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 5. Ingredients (no dependencies) ──────────────────────────────────────
                if (!hasIngredients)
                {
                    await SeedDataFromJsonAsync<Ingredient>("Ingredients.json", _dbContext.Ingredients);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 6. MenuItems (depends on: Categories) ─────────────────────────────────
                if (!hasMenuItems)
                {
                    await SeedDataFromJsonAsync<MenuItem>("MenuItems.json", _dbContext.MenuItems);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 7. Images (depends on: MenuItems) ─────────────────────────────────────
                if (!hasImages)
                {
                    await SeedDataFromJsonAsync<Images>("Images.json", _dbContext.Images);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 8. Recipes (depends on: MenuItems, Ingredients) ───────────────────────
                if (!hasRecipes)
                {
                    await SeedDataFromJsonAsync<Recipe>("Recipes.json", _dbContext.Recipes);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 9. Tables (depends on: Branches) ──────────────────────────────────────
                if (!hasTables)
                {
                    await SeedDataFromJsonAsync<Table>("Tables.json", _dbContext.Tables);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 10. BranchStocks (depends on: Branches, Ingredients) ──────────────────
                if (!hasBranchStocks)
                {
                    await SeedDataFromJsonAsync<BranchStock>("BranchStocks.json", _dbContext.BranchStocks);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 11. Orders (depends on: Branches, Users) ──────────────────────────────
                if (!hasOrders)
                {
                    await SeedDataFromJsonAsync<Order>("Orders.json", _dbContext.Orders);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 12. OrderItems (depends on: Orders, MenuItems) ────────────────────────
                if (!hasOrderItems)
                {
                    await SeedDataFromJsonAsync<OrderItem>("OrderItems.json", _dbContext.OrderItems);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 13. KitchenTickets (depends on: Orders) ───────────────────────────────
                if (!hasKitchenTickets)
                {
                    await SeedDataFromJsonAsync<KitchenTicket>("KitchenTickets.json", _dbContext.KitchenTickets);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 14. Payments (depends on: Orders) — 1-to-1 ───────────────────────────
                if (!hasPayments)
                {
                    await SeedDataFromJsonAsync<Payment>("Payments.json", _dbContext.Payments);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 15. Deliveries (depends on: Orders, Users[Driver]) — 1-to-1 ──────────
                if (!hasDeliveries)
                {
                    await SeedDataFromJsonAsync<Delivery>("Deliveries.json", _dbContext.Deliveries);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 16. TableOrders (depends on: Tables, Orders) — 1-to-1 ────────────────
                if (!hasTableOrders)
                {
                    await SeedDataFromJsonAsync<TableOrder>("TableOrders.json", _dbContext.TableOrders);
                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }
        }

        #region Helper Methods
        private async Task SeedDataFromJsonAsync<T>(string fileName, DbSet<T> dbSet) where T : BaseEntity
        {

            var filePath = string.Empty; //@"..\RMS.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if(isDevelopment) 
                filePath = Path.Combine("..", "RMS.Persistence", "Data", "DataSeed", "JSONFiles", fileName);
            else 
                filePath = Path.Combine("Data", "DataSeed", "JSONFiles", fileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {fileName} Is Not Exists");

            try
            {
                using var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
                if (data is not null)
                {
                    await dbSet.AddRangeAsync(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading JSON File : {ex}");
            }
        }
        #endregion
    }
}
