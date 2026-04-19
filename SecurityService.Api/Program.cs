using SecurityService.Api;
using SecurityService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias
builder.Services.AddInfrastructure(builder.Configuration); // Configura DB y HTTP
builder.Services.AddPresentation(builder.Configuration); // Configura Auth y Swagger

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Security API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();