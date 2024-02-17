namespace db.v1.main.DTOs.Keyboard
{
    public sealed record InsertKeyboardDTO(Guid OwnerID, Guid SwitchTypeID, Guid BoxTypeID,
                                           string Title, string? Description, string FilePath, double CreationDate);
}