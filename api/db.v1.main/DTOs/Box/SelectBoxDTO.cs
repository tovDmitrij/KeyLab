namespace db.v1.main.DTOs.Box
{
    public sealed record SelectBoxDTO(
        Guid ID, Guid TypeID, string TypeTitle,
        string Title, string PreviewName, double CreationDate);
}