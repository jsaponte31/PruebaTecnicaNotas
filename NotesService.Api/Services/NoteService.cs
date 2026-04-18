using Microsoft.EntityFrameworkCore;
using NotesService.Infrastructure.Data;
using NotesService.Domain.Entities;
using NotesService.Domain.DTOs;
using NotesService.Domain.Interfaces;

namespace NotesService.Api.Services;

public class NoteService : INoteService
{
    private readonly NotesDbContext _context;

    public NoteService(NotesDbContext context) => _context = context;

    public async Task<IEnumerable<NoteResponseDto>> GetNotesAsync()
    {
        return await _context.Notes
            .Select(n => new NoteResponseDto(n.Id, n.Title, n.Content, n.CreatedBy, n.CreatedAt))
            .ToListAsync();
    }

    public async Task<NoteResponseDto?> GetNotesByIdAsync(int id)
    {
        return await _context.Notes
            .Where(n => n.Id == id)
            .Select(n => new NoteResponseDto(
                n.Id,
                n.Title,
                n.Content,
                n.CreatedBy,
                n.CreatedAt))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CreateNoteAsync(NoteCreateDto dto, string username)
    {
        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            CreatedBy = username,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Notes.Add(note);
        var success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
            //Llamada al Stored Procedure
            await _context.Database.ExecuteSqlRawAsync(
                "CALL sp_record_log({0}, {1}, {2})",
                "/api/notes", username, "CREATE_NOTE");
        }
        return success;
    }

    public async Task<bool> DeleteNoteAsync(int id, string username)
    {
        // Buscamos por Id y por CreatedBy para asegurarnos de que el usuario solo pueda borrar sus propias notas
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

        if (note == null) return false;

        // Marcamos la nota como eliminada en lugar de borrarla físicamente
        note.IsDeleted = true;

        var success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL sp_record_log({0}, {1}, {2})",
                $"/api/notes/{id}", username, "DELETE_NOTE");
        }
        return success;
    }
}
