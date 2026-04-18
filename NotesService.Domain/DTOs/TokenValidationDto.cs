namespace NotesService.Domain.DTOs;

public class TokenValidationResultDto
{
    public bool IsValid { get; set; }
    public string? Username { get; set; }
    public string? UserId { get; set; }
    public string? ErrorMessage { get; set; }
}
