namespace api.v1.main.DTOs.Keycap
{
    public sealed record PostKeycapDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid KitID, Guid UserID);
}