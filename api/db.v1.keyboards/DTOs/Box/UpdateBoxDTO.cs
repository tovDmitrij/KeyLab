namespace db.v1.keyboards.DTOs.Box
{
    public sealed record UpdateBoxDTO(Guid BoxID, string Title, string FileName, string PreviewName);
}