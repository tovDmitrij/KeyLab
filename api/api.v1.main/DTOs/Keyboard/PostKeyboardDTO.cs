namespace api.v1.main.DTOs.Keyboard
{
    public sealed record PostKeyboardDTO(IFormFile? File, string Title, 
                                        Guid UserID, Guid BoxTypeID, Guid SwitchTypeID);
}