
using ECommerceApi.Database;
using ECommerceApi.Mapping;
using ECommerceApi.Repositories;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ECommerceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Ignore circular references
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // Connecting to the database
            builder.Services.AddDbContext<ECommerceDBContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
                );

            // AutoMap dependency
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Configure logging
            builder.Logging.ClearProviders(); // Clean existent logs providers
            builder.Logging.AddConsole(); // Add log how provider

            // Configure dependencies injection
            builder.Services.AddScoped<IOrdersRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

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
