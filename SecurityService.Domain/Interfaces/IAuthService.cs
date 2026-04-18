using SecurityService.Domain.DTOs;

namespace SecurityService.Domain.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(LoginRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}