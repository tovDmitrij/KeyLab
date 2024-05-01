namespace db.v1.keyboards.DTOs
{
    public sealed record SelectBoxDTO(Guid ID, Guid TypeID, string TypeTitle, string Title, double CreationDate);
}