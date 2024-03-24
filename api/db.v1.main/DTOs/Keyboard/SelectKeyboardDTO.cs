namespace db.v1.main.DTOs.Keyboard
{
    public sealed record SelectKeyboardDTO(Guid ID, Guid BoxTypeID, string BoxTypeTitle, Guid SwitchTypeID, string SwitchTypeTitle,
                                           string Title, double CreationDate);
}