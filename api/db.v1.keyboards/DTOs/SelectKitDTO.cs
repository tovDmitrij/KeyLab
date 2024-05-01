namespace db.v1.keyboards.DTOs
{
    public sealed record SelectKitDTO(Guid ID, Guid BoxTypeID, string Title, double CreationDate);
}