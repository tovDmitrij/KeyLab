namespace api.v1.main.DTOs.Keyboard
{
    public sealed record PutKeyboardDTO(IFormFile? File, string Title, 
                                           Guid UserID, Guid KeyboardID, Guid BoxTypeID, Guid SwitchTypeID);
}