using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using NotesService.Infrastructure.Data;
using NotesService.Domain.Interfaces;
using NotesService.Api.Services;
using NotesService.Api.Configuration;
using NotesService.Api.Middlewares;

namespace NotesService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Base de Datos
        services.AddDbContext<NotesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("NotesConnection")));

        // 2. Registro del Servicio
        services.AddScoped<INoteService, NoteService>();

        // 3. HttpClient para llamadas a SecurityService
        services.AddHttpClient<ITokenValidationService, TokenValidationService>();

        // 4. Servicio de Validación de Tokens contra SecurityService
        services.AddScoped<ITokenValidationService, TokenValidationService>();

        // 5. Autenticación personalizada que valida contra SecurityService
        services.AddAuthentication("SecurityServiceAuth")
            .AddScheme<AuthenticationSchemeOptions, SecurityServiceAuthenticationHandler>(
                "SecurityServiceAuth", options => { });

        services.AddAuthorization();
        services.AddSwaggerConfiguration();

        return services;
    }
}