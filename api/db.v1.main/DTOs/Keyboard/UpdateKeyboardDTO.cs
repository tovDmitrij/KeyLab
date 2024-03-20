namespace db.v1.main.DTOs.Keyboard
{
    public sealed record UpdateKeyboardDTO(Guid KeyboardID, Guid SwitchTypeID, Guid BoxTypeID, string Title, 
                                           string FileName, string PreviewName);
}