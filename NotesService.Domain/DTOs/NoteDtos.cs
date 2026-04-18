namespace NotesService.Domain.DTOs;

public record NoteCreateDto(string Title, string Content);
public record NoteResponseDto(int Id, string Title, string Content, string CreatedBy, DateTime CreatedAt);