namespace db.v1.keyboards.DTOs.Kit
{
    public sealed record SelectKitDTO(Guid ID, Guid BoxTypeID, string Title, double CreationDate);
}