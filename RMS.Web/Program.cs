
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Persistence.Data.Contexts;
using RMS.Persistence.Data.DataSeed;
using RMS.Persistence.Repositories;
using RMS.Web.Extensions;

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

            var app = builder.Build();
            #region Data Seeding

            await app.MigrateDatabaseAsync();
            await app.SeedDatabaseAsync();

            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
