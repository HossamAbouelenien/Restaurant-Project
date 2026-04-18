using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence;
using RMS.Persistence.Data.Contexts;
using RMS.Persistence.Data.DataSeed;
using RMS.Persistence.Repositories;
using RMS.Persistence.Repositries;
using RMS.Presentation.Hubs;
using RMS.Services;
using RMS.Services.BasketService;
using RMS.Services.BranchServices;
using RMS.Services.BranchStockServices;
using RMS.Services.CacheServices;
using RMS.Services.CategoryServices;
using RMS.Services.DeliveryServices;
using RMS.Services.EmailServices;
using RMS.Services.IdentityService;
using RMS.Services.IngredientServices;
using RMS.Services.KitchenServices;
using RMS.Services.MappingProfiles;
using RMS.Services.MenuItemsServices;
using RMS.Services.NotificationServices;
using RMS.Services.OrderServices;
using RMS.Services.RecipeServices;
using RMS.Services.ReportServices;
using RMS.Services.TableServices;
using RMS.Services.UserServices;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.ICategoriesService;
using RMS.ServicesAbstraction.IDeliveryServices;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.ServicesAbstraction.IIdentityService;
using RMS.ServicesAbstraction.IKitchenServices;
using RMS.ServicesAbstraction.IUserServices;
using RMS.ServicesAbstraction.Notifications;
using RMS.Web.Extensions;
using StackExchange.Redis;
using System.Text;
namespace RMS.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddAutoMapper(X => X.AddProfile<MenuItemProfile>(), typeof(MenuItemProfile).Assembly);
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();


            //================= Mahmoud (45 : 60) =================







            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IRealTimeNotifier, RealTimeNotifier>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            //================= Amr (60 : 75) =====================












            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IBranchStockService, BranchStockService>();
            //================= Mustafa (75 : 150) =================
            builder.Services.AddIdentity<User, IdentityRole>()
                          .AddEntityFrameworkStores<AppDbContext>()
                          .AddDefaultTokenProviders();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var key = builder.Configuration.GetValue<string>("JwtSettings:Secret");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
            }
                 });
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IKitchenService, KitchenService>();
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            builder.Services.AddScoped<IEmailService, EmailService>();












            //================= Hossam (150 : 165) =================

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });




            builder.Services.AddScoped<ICategoryService,CategoryService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();


            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });



            //================= Arwa (165 : 180) =================
            builder.Services.AddScoped<IIngredientService, IngredientService>();
            builder.Services.AddScoped<IReportService, ReportService>();












            //================= Areej (180 : 200) =================

            builder.Services.AddScoped<IBranchService, BranchServices>();
            builder.Services.AddScoped<ITableService, TableService>();


















            //=====================================================





            var app = builder.Build();

            #region Data Seeding

            await app.MigrateDatabaseAsync();
            await app.SeedDatabaseAsync();

            #endregion Data Seeding

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<NotificationHub>("/hubs/notifications");

            app.MapControllers();

            app.Run();
        }
    }
}