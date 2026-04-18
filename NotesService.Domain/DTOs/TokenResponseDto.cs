namespace NotesService.Domain.DTOs;

public class TokenResponseDto
{
    public bool Valid { get; set; }
    public string? Username { get; set; }
    public string? UserId { get; set; }
}
