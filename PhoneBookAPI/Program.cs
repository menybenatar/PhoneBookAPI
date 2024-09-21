using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PhoneBookAPI.Data;
using PhoneBookAPI.Helpers; // Namespace where MappingProfile is located
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Services;

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

            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

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
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}