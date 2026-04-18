namespace NotesService.Domain.Entities;

public class ApiLog
{
    public int Id { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}