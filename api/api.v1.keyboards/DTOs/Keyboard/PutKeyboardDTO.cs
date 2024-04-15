namespace api.v1.keyboards.DTOs.Keyboard
{
    public sealed record PutKeyboardDTO(IFormFile? File, IFormFile? Preview, string? Title, 
                                        Guid UserID, Guid KeyboardID, Guid BoxTypeID, Guid SwitchTypeID);
}