namespace db.v1.main.DTOs.Keycap
{
    public sealed record UpdateKeycapDTO(Guid KeycapID, string Title, string FileName, string PreviewName);
}