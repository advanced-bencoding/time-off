
using Auth.Api.Data;
using Auth.Api.DTOs;
using Auth.Api.Middleware;
using Auth.Api.Repositories;
using Auth.Api.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Auth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllers();

            // options
            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<RefreshTokenConfig>(builder.Configuration.GetSection("RefreshToken"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!);
            });

            builder.Services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();

            // repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

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
