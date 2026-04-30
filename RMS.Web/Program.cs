using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RMS.Domain.Contracts;
using RMS.Persistence.Data.Contexts;
using RMS.Persistence.Data.DataSeed;
using RMS.Persistence.Repositories;
using RMS.Persistence.Repositries;
using RMS.Presentation.Hubs.Notification;
using RMS.Presentation.Hubs.RestaurantHub;
using RMS.Services.MappingProfiles;
using RMS.Services.Services.AiServices;
using RMS.Services.Services.AiServices.NutritionServices;
using RMS.Services.Services.AiServices.NutritionServices.Options;
using RMS.Services.Services.BasketService;
using RMS.Services.Services.BranchServices;
using RMS.Services.Services.BranchStockServices;
using RMS.Services.Services.CacheServices;
using RMS.Services.Services.CategoryServices;
using RMS.Services.Services.DeliveryServices;
using RMS.Services.Services.EmailServices;
using RMS.Services.Services.IdentityService;
using RMS.Services.Services.ImageServices;
using RMS.Services.Services.IngredientServices;
using RMS.Services.Services.KitchenServices;
using RMS.Services.Services.MenuItemServices;
using RMS.Services.Services.NotificationServices;
using RMS.Services.Services.OrderServices;
using RMS.Services.Services.RecipeServices;
using RMS.Services.Services.ReportServices;
using RMS.Services.Services.TableServices;
using RMS.Services.Services.UserServices;
using RMS.ServicesAbstraction.IServices.IAiServices;
using RMS.ServicesAbstraction.IServices.IBasketServices;
using RMS.ServicesAbstraction.IServices.IBranchServices;
using RMS.ServicesAbstraction.IServices.IBranchStockServices;
using RMS.ServicesAbstraction.IServices.ICacheServices;
using RMS.ServicesAbstraction.IServices.ICategoryServices;
using RMS.ServicesAbstraction.IServices.IDeliveryServices;
using RMS.ServicesAbstraction.IServices.IEmailServices;
using RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices;
using RMS.ServicesAbstraction.IServices.IHubServices.IRestaurantNotifier;
using RMS.ServicesAbstraction.IServices.IIdentityService;
using RMS.ServicesAbstraction.IServices.IImageServices;
using RMS.ServicesAbstraction.IServices.IIngredientServices;
using RMS.ServicesAbstraction.IServices.IKitchenServices;
using RMS.ServicesAbstraction.IServices.IMenuItemServices;
using RMS.ServicesAbstraction.IServices.IOrderServices;
using RMS.ServicesAbstraction.IServices.IPaymentServices;
using RMS.ServicesAbstraction.IServices.IPaymobServices;
using RMS.ServicesAbstraction.IServices.IRecipeServices;
using RMS.ServicesAbstraction.IServices.IReportServices;
using RMS.ServicesAbstraction.IServices.ITableServices;
using RMS.ServicesAbstraction.IServices.IUserServices;
using RMS.Shared.Utility;
using RMS.Web.Extensions;
using RMS.Web.Factories;
using Serilog.Context;
using StackExchange.Redis;
using System.Globalization;
using System.Security.Claims;
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

            builder.Services.Configure<OpenAiOptions>(
            builder.Configuration.GetSection(OpenAiOptions.SectionName));

            builder.Services.AddHttpClient<INutritionAiService, NutritionAiService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(45);
                client.BaseAddress = new Uri("https://api.openai.com/");

              
                var apiKey = builder.Configuration["OpenAI:ApiKey"];
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            });

            builder.Services.AddScoped<INutritionService, NutritionService>();
            builder.Services.AddScoped<INutritionRepository, NutritionRepository>();
            builder.Services.AddScoped<IRestaurantNotifier, RestaurantNotifier>();
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
            builder.Services.AddIdentity<RMS.Domain.Entities.User, IdentityRole>()
                          .AddEntityFrameworkStores<AppDbContext>()
                          .AddDefaultTokenProviders();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var key = builder.Configuration.GetValue<string>("JwtSettings:Secret");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                
            })
           
            .AddCookie(options =>                         
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.HttpOnly = true;
                
            })
             .AddGoogle(options =>
             {
                 options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                 options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
                 options.CallbackPath = "/signin-google";
                 options.SignInScheme = IdentityConstants.ExternalScheme; 
             })
            .AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
                options.CallbackPath = "/signin-facebook";
                options.SignInScheme = IdentityConstants.ExternalScheme; 
                options.Fields.Add("email");
                options.Fields.Add("name");

                options.Events.OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/api/auth/external-cancelled");
                    return Task.CompletedTask;
                };
            })
                 .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                           
                                var accessTokenFromCookie = context.Request.Cookies["accessToken"];
                                if (!string.IsNullOrEmpty(accessTokenFromCookie))
                                {
                                    context.Token = accessTokenFromCookie;
                                    return Task.CompletedTask;
                                }

                             
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.HttpContext.Request.Path;

                                if (!string.IsNullOrEmpty(accessToken) &&
                                    (path.StartsWithSegments("/hubs/notifications") ||
                                     path.StartsWithSegments("/hubs/restaurant")))
                                {
                                    context.Token = accessToken;
                                }

                                return Task.CompletedTask;
                            
                        }
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
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddHttpClient<IPaymobService, PaymobService>();

            //builder.Host.UseSerilog((context, services, configuration) =>
            //{
            //    configuration.ReadFrom.Configuration(context.Configuration);
            //});
            //builder.Host.UseSerilog((context, services, configuration) =>
            //{
            //    configuration
            //        .ReadFrom.Configuration(context.Configuration)
            //        .ReadFrom.Services(services)
            //        .Enrich.FromLogContext();
            //});

            builder.Services.Configure<PaymobSettings>(
                builder.Configuration.GetSection("Paymob")
            );

            builder.Services.AddHttpClient("RecipeSuggestionClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["AiServices:RecipeSuggestionBaseUrl"]!);
            });

            builder.Services.AddScoped<IAiRecipeService, AiRecipeService>();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;

                var account = new Account(
                    settings.CloudName,
                    settings.ApiKey,
                    settings.ApiSecret);

                return new Cloudinary(account);
            });

            builder.Services.AddScoped<IImageValidator, ImageValidator>();
            builder.Services.AddScoped<IImageStorageStrategy, CloudinaryStorageStrategy>();
            builder.Services.AddScoped<IImageService, ImageService>();


            //================= Hossam (150 : 165) =================

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });




            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();


            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });



            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;


            });



            //================= Arwa (165 : 180) =================
            builder.Services.AddScoped<IIngredientService, IngredientService>();
            builder.Services.AddScoped<IReportService, ReportService>();












            //================= Areej (180 : 200) =================

            builder.Services.AddScoped<IBranchService, BranchServices>();
            builder.Services.AddScoped<ITableService, TableService>();


















            //=====================================================

            #region Localization
            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("en-GB")
                };

                options.DefaultRequestCulture = new RequestCulture("ar-EG");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });




            #endregion

            var app = builder.Build();

            #region Data Seeding

            await app.MigrateDatabaseAsync();
            await app.SeedDatabaseAsync();

            #endregion Data Seeding

            #region Localization Middleware
            var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();
            app.UseRouting();
            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");

            app.Use(async (context, next) =>
            {
                var correlationId = Guid.NewGuid().ToString();

                context.Items["CorrelationId"] = correlationId;

                LogContext.PushProperty("CorrelationId", correlationId);

                await next();
            });

            //app.UseSerilogRequestLogging(options =>
            //{
            //    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed} ms";
            //});


            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<RestaurantHub>("/hubs/restaurant");
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.MapControllers();

            app.Run();
        }
    }
}