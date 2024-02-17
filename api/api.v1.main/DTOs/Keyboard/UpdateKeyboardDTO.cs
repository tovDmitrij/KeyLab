namespace api.v1.main.DTOs.Keyboard
{
    public sealed record UpdateKeyboardDTO(IFormFile? File, string Title, string? Description, 
                                           Guid UserID, Guid KeyboardID, Guid BoxTypeID, Guid SwitchTypeID);
}