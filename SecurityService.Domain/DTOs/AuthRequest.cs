namespace SecurityService.Domain.DTOs;

// DTO para recibir las credenciales en el login
public record LoginRequest(string Username, string Password);

// DTO para la respuesta con el token generado
public record AuthResponse(string Token, string Username);

// DTO para la validación que pedirá el NotesService
public record TokenValidationRequest(string Token);
public record TokenValidationResponse(bool IsValid, string Username);
