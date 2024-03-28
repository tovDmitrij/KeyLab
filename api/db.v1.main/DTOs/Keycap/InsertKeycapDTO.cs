namespace db.v1.main.DTOs.Keycap
{
    public sealed record InsertKeycapDTO(Guid KitID, string Title, string FileName, string PreviewName, double CreationDate);
}