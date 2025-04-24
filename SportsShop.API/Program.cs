
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SportsShop.Repository.Data;
using System.Diagnostics;
using MapsterMapper;
using SportsShop.Service.Helpers;
using System.Reflection;
using SportsShop.API.Helpers;
using Mapster;
using SportsShop.API.Middlewares;
using Hangfire.States;

namespace SportsShop.API
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


            builder.Services.AddDbContext<ShopContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging();
            });


            #region Autofac Registration
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
                builder.RegisterModule(new AutofacModule()));
            #endregion

            #region MediatR Configuration
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            #region Mapping
            builder.Services.AddMapster();

            MappingProfile.RegisterMappings();



            #endregion

            var app = builder.Build();

            MapperHelper.Mapper = app.Services.GetService<IMapper>();

            try
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ShopContext>();
                await context.Database.MigrateAsync();
                await ShopContextSeed.SeedAsync(context);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


            app.UseMiddleware<TransactionMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
