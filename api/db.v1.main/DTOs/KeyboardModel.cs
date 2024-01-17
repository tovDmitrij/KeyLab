namespace db.v1.main.DTOs
{
    public sealed record KeyboardModel(Guid id, string title, string? description, double creationDate);
}