namespace db.v1.main.DTOs
{
    public sealed record KeyboardInfoDTO(
        Guid ID, Guid BoxTypeID, string BoxTypeTitle,
        Guid SwitchTypeID, string SwitchTypeTitle,
        string Title, string? Description, double CreationDate);
}