using FinanceTracker.Infra.Identity.Repositories;
using FinanceTracker.Domain.Identity.Interfaces;
using FinanceTracker.Application.Services;
using FinanceTracker.Infra.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinanceTracker.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using FinanceTracker.Infra.Identity.Services;

namespace FinanceTracker.Presentation.Exensions;

public static class AddIdentityExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value!))
            };
        });

        // Configure Identity
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        services.AddScoped(sp => new JwtTokenGenerator(configuration.GetSection("Jwt:Key").Value!));
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}