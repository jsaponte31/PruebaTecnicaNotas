using Microsoft.EntityFrameworkCore;
using SecurityService.Api.Data;
using SecurityService.Api.Authentication;
using SecurityService.Api.Services;
using SecurityService.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SecurityService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuración de la Base de Datos
        var connectionString = configuration.GetConnectionString("SecurityConnection");
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Servicios de Lógica de Negocio
        services.AddScoped<IAuthService, AuthService>();

        // 3. Utilidades de Autenticación (JWT)
        services.AddScoped<JwtProvider>();

        // 4. Configuración de Autenticación JWT
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

        // 5. Autorización (Opcional pero recomendado aquí)
        services.AddAuthorization();

        services.AddSwaggerGen();

        return services;
    }
}
