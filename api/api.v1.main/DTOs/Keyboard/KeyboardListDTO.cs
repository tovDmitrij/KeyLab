namespace api.v1.main.DTOs.Keyboard
{
    public sealed record KeyboardListDTO(
        Guid ID, Guid BoxTypeID, string BoxTypeTitle,
        Guid SwitchTypeID, string SwitchTypeTitle,
        string Title, string Preview, double CreationDate);
}