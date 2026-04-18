using NotesService.Domain.DTOs;

namespace NotesService.Domain.Interfaces;

public interface ITokenValidationService
{
    Task<TokenValidationResultDto> ValidateTokenAsync(string token);
}
