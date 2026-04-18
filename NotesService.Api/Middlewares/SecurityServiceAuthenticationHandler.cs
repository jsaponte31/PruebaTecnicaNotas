using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotesService.Domain.Interfaces;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace NotesService.Api.Middlewares;

public class SecurityServiceAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ITokenValidationService _tokenValidationService;
    private string _errorMessage = string.Empty;

    public SecurityServiceAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ITokenValidationService tokenValidationService)
        : base(options, logger, encoder, clock)
    {
        _tokenValidationService = tokenValidationService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Extraer el token del header Authorization
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            _errorMessage = "No Authorization header";
            return AuthenticateResult.Fail(_errorMessage);
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            _errorMessage = "Invalid Authorization header format";
            return AuthenticateResult.Fail(_errorMessage);
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        // Validar el token contra SecurityService
        var validationResult = await _tokenValidationService.ValidateTokenAsync(token);

        if (!validationResult.IsValid)
        {
            _errorMessage = validationResult.ErrorMessage ?? "Token validation failed";
            return AuthenticateResult.Fail(_errorMessage);
        }

        // Crear claims basados en la respuesta de SecurityService
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, validationResult.UserId ?? ""),
            new Claim(ClaimTypes.Name, validationResult.Username ?? "")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = 401;

        var errorResponse = new
        {
            message = _errorMessage == string.Empty ? "Unauthorized" : _errorMessage
        };

        await Response.WriteAsJsonAsync(errorResponse);
    }
}
