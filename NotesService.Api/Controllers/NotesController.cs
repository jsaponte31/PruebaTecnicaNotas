using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesService.Domain.DTOs;
using NotesService.Domain.Interfaces;
using System.Security.Claims;

namespace NotesService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _noteService.GetNotesAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _noteService.GetNotesByIdAsync(id);

        if (note == null)
            return NotFound(new { message = "La nota no existe o fue eliminada" });

        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NoteCreateDto dto)
    {
        // Extraemos el nombre de usuario del claim del Token
        var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized(new { message = "Usuario no identificado en el token" });

        var success = await _noteService.CreateNoteAsync(dto, username);

        return success
            ? CreatedAtAction(nameof(GetById), new { id = 0 }, new { message = "Nota creada con éxito" })
            : BadRequest(new { message = "No se pudo crear la nota" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var success = await _noteService.DeleteNoteAsync(id, username);

        return success
            ? Ok(new { message = "Nota eliminada correctamente" })
            : NotFound(new { message = "No se encontró la nota o no tienes permisos para borrarla" });
    }
}