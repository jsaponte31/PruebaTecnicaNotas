using System.Net.Http.Headers;
using NotesService.Domain.DTOs;
using NotesService.Domain.Interfaces;

namespace NotesService.Api.Services;

public class TokenValidationService : ITokenValidationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenValidationService> _logger;

    public TokenValidationService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<TokenValidationService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<TokenValidationResultDto> ValidateTokenAsync(string token)
    {
        try
        {
            var securityServiceUrl = _configuration["SecurityServiceUrl"];
            if (string.IsNullOrEmpty(securityServiceUrl))
            {
                _logger.LogError("SecurityServiceUrl no configurado en appsettings");
                return new TokenValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "SecurityServiceUrl no configurado"
                };
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{securityServiceUrl}/api/auth/validate?token={Uri.EscapeDataString(token)}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Token validation failed with status {response.StatusCode}");
                return new TokenValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Token inválido"
                };
            }

            var content = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            
            if (content == null)
            {
                return new TokenValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Respuesta inválida del servicio de seguridad"
                };
            }

            return new TokenValidationResultDto
            {
                IsValid = content.Valid,
                Username = content.Username,
                UserId = content.UserId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error validating token: {ex.Message}");
            return new TokenValidationResultDto
            {
                IsValid = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
