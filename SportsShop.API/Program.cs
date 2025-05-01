
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SportsShop.Repository.Data;
using System.Diagnostics;

using SportsShop.Service.Helpers;
using System.Reflection;
using SportsShop.API.Helpers;

using SportsShop.API.Middlewares;
using Hangfire.States;
using AutoMapper;
using StackExchange.Redis;
using SportsShop.Core.Entities;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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

            #region Redis

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                if (connection == null) throw new Exception("Can not get Redis connection string");
                return ConnectionMultiplexer.Connect(connection);
            });

            #endregion

            #region Fluent validation
            //builder.Services.AddFluentVa(); // For auto model validation
            //builder.Services.AddFluentValidationClientsideAdapters();
            #endregion

            #region Autofac Registration
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
                builder.RegisterModule(new AutofacModule()));
            #endregion

            #region MediatR Configuration
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            builder.Services.AddAuthorization();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ShopContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(double.Parse(builder.Configuration["JWT:DurationInDays"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Read token from cookie instead of Authorization header
                            var token = context.Request.Cookies["access_token"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddCors();

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
            app.UseMiddleware<ExceptionMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                .WithOrigins("http://localhost:4200", "https://localhost:4200"));

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();



            app.Run();
        }
    }
}
