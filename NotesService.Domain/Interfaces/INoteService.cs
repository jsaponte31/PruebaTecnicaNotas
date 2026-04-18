using NotesService.Domain.DTOs;

namespace NotesService.Domain.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteResponseDto>> GetNotesAsync();
    Task<NoteResponseDto?> GetNotesByIdAsync(int id);
    Task<bool> CreateNoteAsync(NoteCreateDto dto, string username);
    Task<bool> DeleteNoteAsync(int id, string username);
}