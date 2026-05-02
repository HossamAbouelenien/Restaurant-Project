using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence.Data.Contexts;
using RMS.Shared.DTOs.Utility;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RMS.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(AppDbContext dbContext,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var customerId = string.Empty;

                var hasBranches = await _dbContext.Branches.AnyAsync();
                var hasCategories = await _dbContext.Categories.AnyAsync();
                var hasIngredients = await _dbContext.Ingredients.AnyAsync();
                var hasMenuItems = await _dbContext.MenuItems.AnyAsync();
                var hasRecipes = await _dbContext.Recipes.AnyAsync();
                var hasTables = await _dbContext.Tables.AnyAsync();
                var hasBranchStocks = await _dbContext.BranchStocks.AnyAsync();
                var hasOrders = await _dbContext.Orders.AnyAsync();
                var hasOrderItems = await _dbContext.OrderItems.AnyAsync();
                var hasKitchenTickets = await _dbContext.KitchenTickets.AnyAsync();
                var hasPayments = await _dbContext.Payments.AnyAsync();
                var hasDeliveries = await _dbContext.Deliveries.AnyAsync();
                var hasTableOrders = await _dbContext.TableOrders.AnyAsync();

                if (hasBranches && hasCategories && hasIngredients && hasMenuItems &&
                    hasRecipes && hasTables && hasBranchStocks && hasOrders && hasOrderItems && hasKitchenTickets && hasPayments
                    && hasDeliveries && hasTableOrders) return;

                // ── 1. Branches (no dependencies) ─────────────────────────────────────────
                if (!hasBranches)
                {
                    await SeedDataFromJsonAsync<Branch>("Branches.json", _dbContext.Branches);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 2. Categories (no dependencies) ───────────────────────────────────────
                if (!hasCategories)
                {
                    await SeedDataFromJsonAsync<Category>("Categories.json", _dbContext.Categories);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 3. Ingredients (no dependencies) ──────────────────────────────────────
                if (!hasIngredients)
                {
                    await SeedDataFromJsonAsync<Ingredient>("Ingredients.json", _dbContext.Ingredients);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 4. MenuItems (depends on: Categories) ─────────────────────────────────
                if (!hasMenuItems)
                {
                    await SeedDataFromJsonAsync<MenuItem>("MenuItems.json", _dbContext.MenuItems);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 5. Recipes (depends on: MenuItems, Ingredients) ───────────────────────
                if (!hasRecipes)
                {
                    await SeedDataFromJsonAsync<Recipe>("Recipes.json", _dbContext.Recipes);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 6. Tables (depends on: Branches) ──────────────────────────────────────
                if (!hasTables)
                {
                    await SeedDataFromJsonAsync<Table>("Tables.json", _dbContext.Tables);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 7. BranchStocks (depends on: Branches, Ingredients) ──────────────────
                if (!hasBranchStocks)
                {
                    await SeedDataFromJsonAsync<BranchStock>("BranchStocks.json", _dbContext.BranchStocks);
                    await _dbContext.SaveChangesAsync();
                }

                // ── 8. Orders (depends on: Branches, Users) ──────────────────────────────
                //if (!hasOrders)
                //{
                //    await SeedDataFromJsonAsync<Order>("Orders.json", _dbContext.Orders);
                //    await _dbContext.SaveChangesAsync();
                //}

                // ── 9. OrderItems (depends on: Orders, MenuItems) ────────────────────────
                //if (!hasOrderItems)
                //{
                //    await SeedDataFromJsonAsync<OrderItem>("OrderItems.json", _dbContext.OrderItems);
                //    await _dbContext.SaveChangesAsync();
                //}

                // ── 10. KitchenTickets (depends on: Orders) ───────────────────────────────
                //if (!hasKitchenTickets)
                //{
                //    await SeedDataFromJsonAsync<KitchenTicket>("KitchenTickets.json", _dbContext.KitchenTickets);
                //    await _dbContext.SaveChangesAsync();
                //}

                // ── 11. Payments (depends on: Orders) — 1-to-1 ───────────────────────────
                //if (!hasPayments)
                //{
                //    await SeedDataFromJsonAsync<Payment>("Payments.json", _dbContext.Payments);
                //    await _dbContext.SaveChangesAsync();
                //}

                // ── 12. Deliveries (depends on: Orders, Users[Driver]) — 1-to-1 ──────────
                //if (!hasDeliveries)
                //{
                //    await SeedDataFromJsonAsync<Delivery>("Deliveries.json", _dbContext.Deliveries);
                //    await _dbContext.SaveChangesAsync();
                //}

                // ── 13. TableOrders (depends on: Tables, Orders) — 1-to-1 ────────────────
                //if (!hasTableOrders)
                //{
                //    await SeedDataFromJsonAsync<TableOrder>("TableOrders.json", _dbContext.TableOrders);
                //    await _dbContext.SaveChangesAsync();
                //}
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
            if (isDevelopment)
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
                    // Special handling for Orders to set CustomerId based on email (since JSON won't have FK references)
                    if ( typeof(T) == typeof(Order) && fileName == "Orders.json")
                    {
                        foreach (var item in data)
                        {
                            var order = item as Order;
                            var user = await _userManager.FindByEmailAsync("areej@gmai.com");
                            if (user is not null)
                            {
                                order.UserId = user.Id;
                                order.User = null;
                            }
                        }
                    }
                    if (typeof(T) == typeof(Delivery) && fileName == "Deliveries.json")
                    {
                        foreach (var item in data)
                        {
                            var delivery = item as Delivery;
                            var user = await _userManager.FindByEmailAsync("areej@gmai.com");
                            if (user is not null)
                            {
                                delivery.DriverId = user.Id;
                                delivery.Driver = null;
                            }
                        }
                    }
                    await dbSet.AddRangeAsync(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading JSON File : {ex}");
            }
        }

        #endregion Helper Methods

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                var customerRole = string.Empty;
                var adminRole = string.Empty;
                var cahierRole = string.Empty;
                var waiterRole = string.Empty;
                var driverRole = string.Empty;
                var chefRole = string.Empty;

                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Waiter));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Driver));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Chef));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Cashier));
                    customerRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Customer));
                    adminRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Admin));
                    cahierRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Cashier));
                    waiterRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Waiter));
                    driverRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Driver));
                    chefRole = await _roleManager.GetRoleIdAsync(new IdentityRole(SD.Role_Chef));
                }
                if (!_userManager.Users.Any())
                {
                    var password = "Mm123@#";

                    var admin = new User
                    {
                        Name = "admin",
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        PhoneNumber = "0123456789",
                        RoleId = adminRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    var cashier = new User
                    {
                        Name = "cashier",
                        Email = "cashier@gmail.com",
                        UserName = "cashier",
                        PhoneNumber = "0123456789",
                        RoleId = cahierRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    var chef = new User
                    {
                        Name = "chef",
                        Email = "chef@gmail.com",
                        UserName = "chef",
                        PhoneNumber = "0123456789",
                        RoleId = chefRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    var customer = new User
                    {
                        Name = "customer",
                        Email = "customer@gmail.com",
                        UserName = "customer",
                        PhoneNumber = "0123456789",
                        RoleId = customerRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    var driver = new User
                    {
                        Name = "driver",
                        Email = "driver@gmail.com",
                        UserName = "driver",
                        PhoneNumber = "0123456789",
                        RoleId = driverRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    var waiter = new User
                    {
                        Name = "waiter",
                        Email = "waiter@gmail.com",
                        UserName = "waiter",
                        PhoneNumber = "0123456789",
                        RoleId = waiterRole,
                        EmailConfirmed = true,
                        //BranchId = 1
                    };

                    await _userManager.CreateAsync(admin, password);
                    await _userManager.CreateAsync(cashier, password);
                    await _userManager.CreateAsync(chef, password);
                    await _userManager.CreateAsync(customer, password);
                    await _userManager.CreateAsync(driver, password);
                    await _userManager.CreateAsync(waiter, password);
                    await _userManager.AddToRoleAsync(admin, SD.Role_Admin);
                    await _userManager.AddToRoleAsync(cashier, SD.Role_Cashier);
                    await _userManager.AddToRoleAsync(chef, SD.Role_Chef);
                    await _userManager.AddToRoleAsync(customer, SD.Role_Customer);
                    await _userManager.AddToRoleAsync(driver, SD.Role_Driver);
                    await _userManager.AddToRoleAsync(waiter, SD.Role_Waiter);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}