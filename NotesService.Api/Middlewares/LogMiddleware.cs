using Microsoft.EntityFrameworkCore;
using NotesService.Infrastructure.Data;
using System.Security.Claims;

namespace NotesService.Api.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, NotesDbContext dbContext)
    {
        // Dejamos que la petición siga su curso
        await _next(context);

        if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
        {
            var endpoint = context.Request.Path;
            var action = context.Request.Method;
            // Extraemos el nombre del usuario del Token JWT
            var username = context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";

            // LLAMADA AL PROCEDIMIENTO ALMACENADO
            await dbContext.Database.ExecuteSqlRawAsync(
                "CALL sp_record_log({0}, {1}, {2})",
                endpoint, username, action);
        }
    }
}
