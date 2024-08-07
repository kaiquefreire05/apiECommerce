
using ECommerceApi.Database;
using ECommerceApi.Mapping;
using ECommerceApi.Repositories;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerce - API", // Title API
                    Version = "v1" // API Version
                });

                // Setting security schema for JWT autentication
                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter with JWT Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                // Add security defintion to Swagger
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);

                // Defining security requimerents to API
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securitySchema, new string[] {} }
                });

            });

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

            // Configure Authentication
            var jwtSettings = builder.Configuration.GetSection("jwt");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["secretkey"]);
            var issuer = jwtSettings["issuer"];
            var audience = jwtSettings["audience"];

            builder.Services.AddAuthentication(o =>
            {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)

                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Adding authentication
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
