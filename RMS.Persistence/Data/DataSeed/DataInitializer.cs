using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence.Data.Contexts;
using System.Text.Json;

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
                // ── 1. Roles (no dependencies) ────────────────────────────────────────────
                var hasRoles = await _dbContext.Roles.AnyAsync();
                if (!hasRoles)
                {
                    await SeedDataFromJsonAsync<Role, int>("Roles.json", _dbContext.Roles);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 2. Branches (no dependencies) ─────────────────────────────────────────
                var hasBranches = await _dbContext.Branches.AnyAsync();
                if (!hasBranches)
                {
                    await SeedDataFromJsonAsync<Branch, int>("Branches.json", _dbContext.Branches);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 3. Users (depends on: Roles, Branches) ────────────────────────────────
                var hasUsers = await _dbContext.Users.AnyAsync();
                if (!hasUsers)
                {
                    await SeedDataFromJsonAsync<User, int>("Users.json", _dbContext.Users);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 4. Categories (no dependencies) ───────────────────────────────────────
                var hasCategories = await _dbContext.Categories.AnyAsync();
                if (!hasCategories)
                {
                    await SeedDataFromJsonAsync<Category, int>("Categories.json", _dbContext.Categories);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 5. Ingredients (no dependencies) ──────────────────────────────────────
                var hasIngredients = await _dbContext.Ingredients.AnyAsync();
                if (!hasIngredients)
                {
                    await SeedDataFromJsonAsync<Ingredient, int>("Ingredients.json", _dbContext.Ingredients);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 6. MenuItems (depends on: Categories) ─────────────────────────────────
                var hasMenuItems = await _dbContext.MenuItems.AnyAsync();
                if (!hasMenuItems)
                {
                    await SeedDataFromJsonAsync<MenuItem, int>("MenuItems.json", _dbContext.MenuItems);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 7. Images (depends on: MenuItems) ─────────────────────────────────────
                var hasImages = await _dbContext.Images.AnyAsync();
                if (!hasImages)
                {
                    await SeedDataFromJsonAsync<Images, int>("Images.json", _dbContext.Images);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 8. Recipes (depends on: MenuItems, Ingredients) ───────────────────────
                var hasRecipes = await _dbContext.Recipes.AnyAsync();
                if (!hasRecipes)
                {
                    await SeedDataFromJsonAsync<Recipe, int>("Recipes.json", _dbContext.Recipes);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 9. Tables (depends on: Branches) ──────────────────────────────────────
                var hasTables = await _dbContext.Tables.AnyAsync();
                if (!hasTables)
                {
                    await SeedDataFromJsonAsync<Table, int>("Tables.json", _dbContext.Tables);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 10. BranchStocks (depends on: Branches, Ingredients) ──────────────────
                var hasBranchStocks = await _dbContext.BranchStocks.AnyAsync();
                if (!hasBranchStocks)
                {
                    await SeedDataFromJsonAsync<BranchStock, int>("BranchStocks.json", _dbContext.BranchStocks);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 11. Orders (depends on: Branches, Users) ──────────────────────────────
                var hasOrders = await _dbContext.Orders.AnyAsync();
                if (!hasOrders)
                {
                    await SeedDataFromJsonAsync<Order, int>("Orders.json", _dbContext.Orders);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 12. OrderItems (depends on: Orders, MenuItems) ────────────────────────
                var hasOrderItems = await _dbContext.OrderItems.AnyAsync();
                if (!hasOrderItems)
                {
                    await SeedDataFromJsonAsync<OrderItem, int>("OrderItems.json", _dbContext.OrderItems);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 13. KitchenTickets (depends on: Orders) ───────────────────────────────
                var hasKitchenTickets = await _dbContext.KitchenTickets.AnyAsync();
                if (!hasKitchenTickets)
                {
                    await SeedDataFromJsonAsync<KitchenTicket, int>("KitchenTickets.json", _dbContext.KitchenTickets);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 14. Payments (depends on: Orders) — 1-to-1 ───────────────────────────
                var hasPayments = await _dbContext.Payments.AnyAsync();
                if (!hasPayments)
                {
                    await SeedDataFromJsonAsync<Payment, int>("Payments.json", _dbContext.Payments);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 15. Deliveries (depends on: Orders, Users[Driver]) — 1-to-1 ──────────
                var hasDeliveries = await _dbContext.Deliveries.AnyAsync();
                if (!hasDeliveries)
                {
                    await SeedDataFromJsonAsync<Delivery, int>("Deliveries.json", _dbContext.Deliveries);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 16. TableOrders (depends on: Tables, Orders) — 1-to-1 ────────────────
                var hasTableOrders = await _dbContext.TableOrders.AnyAsync();
                if (!hasTableOrders)
                {
                    await SeedDataFromJsonAsync<TableOrder, int>("TableOrders.json", _dbContext.TableOrders);
                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }
        }

        #region Helper Methods
        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity
        {
            
            var filePath = @"..\RMS.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {fileName} Is Not Exists");

            try
            {
                using var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
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
