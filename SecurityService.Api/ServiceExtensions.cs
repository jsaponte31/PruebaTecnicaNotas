using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecurityService.Api.Authentication;
using SecurityService.Api.Services;
using SecurityService.Domain.Interfaces;
using System.Text;

namespace SecurityService.Api;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // Servicios de Lógica de Negocio
        services.AddScoped<IAuthService, AuthService>();

        // Utilidades de Autenticación (JWT)
        services.AddScoped<JwtProvider>();

        // Configuración de Autenticación JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        // Autorización
        services.AddAuthorization();

        // Swagger
        services.AddSwaggerGen();

        return services;
    }
}
