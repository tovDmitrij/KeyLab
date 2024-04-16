namespace api.v1.keyboards.DTOs.Keyboard
{
    public sealed record PostKeyboardDTO(IFormFile? File, IFormFile? Preview, string? Title, 
                                        Guid UserID, Guid BoxTypeID, Guid SwitchTypeID);
}