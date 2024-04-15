namespace api.v1.keyboards.DTOs.Keycap
{
    public sealed record PutKeycapDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid KeycapID, Guid UserID);
}