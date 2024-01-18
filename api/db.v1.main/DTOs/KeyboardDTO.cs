namespace db.v1.main.DTOs
{
    public sealed record KeyboardDTO(Guid id, string title, string? description, double creationDate);
}