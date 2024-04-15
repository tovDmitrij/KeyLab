namespace db.v1.keyboards.DTOs.Keycap
{
    public sealed record UpdateKeycapDTO(Guid KeycapID, string Title, string FileName, string PreviewName);
}