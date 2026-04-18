using NotesService.Api;

var builder = WebApplication.CreateBuilder(args);

// Una sola línea para toda la infraestructura
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Notes Service API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// El orden es sagrado para que funcione el [Authorize]
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();