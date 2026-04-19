using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotesService.Infrastructure.Data;

namespace NotesService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Base de Datos
        services.AddDbContext<NotesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("NotesConnection")));

        // HttpClient para llamadas externas si es necesario
        services.AddHttpClient();

        return services;
    }
}