using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityService.Infrastructure.Data;

namespace SecurityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Base de Datos
        var connectionString = configuration.GetConnectionString("SecurityConnection");
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseNpgsql(connectionString));

        // HttpClient para llamadas externas si es necesario
        services.AddHttpClient();

        return services;
    }
}
