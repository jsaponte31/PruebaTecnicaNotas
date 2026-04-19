using Microsoft.AspNetCore.Authentication;
using NotesService.Api.Configuration;
using NotesService.Api.Middlewares;
using NotesService.Api.Services;
using NotesService.Domain.Interfaces;

namespace NotesService.Api;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro de servicios de aplicación
        services.AddScoped<INoteService, NoteService>();
        
        // HttpClient tipado para TokenValidationService (requiere HttpClient inyectado)
        services.AddHttpClient<ITokenValidationService, TokenValidationService>();

        // Autenticación (Es política de la API)
        services.AddAuthentication("SecurityServiceAuth")
            .AddScheme<AuthenticationSchemeOptions, SecurityServiceAuthenticationHandler>(
                "SecurityServiceAuth", options => { });

        services.AddAuthorization();
        services.AddSwaggerConfiguration();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
}
