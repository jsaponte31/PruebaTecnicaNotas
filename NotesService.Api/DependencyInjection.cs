using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NotesService.Infrastructure.Data;
using NotesService.Domain.Interfaces;
using NotesService.Api.Services;

namespace NotesService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Base de Datos (PostgreSQL puerto 5434)
        services.AddDbContext<NotesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("NotesConnection")));

        // 2. Registro del Servicio de Notas (Interfaz e Implementación)
        services.AddScoped<INoteService, NoteService>();

        // 3. Configuración de Seguridad JWT (Para que [Authorize] funcione)
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
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

        services.AddAuthorization();

        return services;
    }
}