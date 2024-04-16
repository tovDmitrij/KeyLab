namespace api.v1.keyboards.DTOs.Keycap
{
    public sealed record PostKeycapDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid KitID, Guid UserID);
}