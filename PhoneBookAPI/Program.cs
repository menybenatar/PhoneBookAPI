using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PhoneBookAPI.Data;
using PhoneBookAPI.Extensions;
using PhoneBookAPI.Helpers; // Namespace where MappingProfile is located
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Services;
using Serilog;

namespace PhoneBookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Configure DbContext to use PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
            builder.Services.AddStackExchangeRedisCache(option =>
            option.Configuration = builder.Configuration.GetConnectionString("Redis"));
            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));


            // Add services to the container.
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                // Run only HTTP for non-development environments (like Docker)
                serverOptions.ListenAnyIP(80); // Listen on port 80
            });
            // Using Serilog for daily rolling file logging in the "Logs" directory; with more time, I would have integrated Elasticsearch for advanced log indexing, searching, and real-time monitoring.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Host.UseSerilog(); 

            // Add Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneBookAPI", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneBookAPI v1");
                    c.RoutePrefix = string.Empty; // Sets Swagger at the app's root (http://localhost:<port>/)
                });
                app.ApplyMigrations();

            }

            // Use CORS globally
            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}