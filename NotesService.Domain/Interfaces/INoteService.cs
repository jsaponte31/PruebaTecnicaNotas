using NotesService.Domain.DTOs;

namespace NotesService.Domain.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteResponseDto>> GetNotesAsync(string username);
    Task<NoteResponseDto?> GetNotesByIdAsync(int id, string username);
    Task<bool> CreateNoteAsync(NoteCreateDto dto, string username);
    Task<bool> DeleteNoteAsync(int id, string username);
}