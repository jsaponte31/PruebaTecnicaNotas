using System.Reflection;
using Microsoft.OpenApi;

namespace NotesService.Api.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // Evita conflicto de schemaId cuando varias clases anidadas se llaman Request/Response
            c.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
            
            // Incluir comentarios XML para descripciones en Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);

            // Definición de seguridad Bearer (JWT). Con Http + Scheme "bearer", Swagger UI envía "Authorization: Bearer <token>" si el usuario pega solo el token.
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT de autorización. Pegue aquí solo el token obtenido en /api/auth/login (sin escribir la palabra Bearer).",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes Service API v1");
            });
        }

        return app;
    }
}
