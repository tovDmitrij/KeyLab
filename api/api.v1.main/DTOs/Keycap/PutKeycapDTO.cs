namespace api.v1.main.DTOs.Keycap
{
    public sealed record PutKeycapDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid KeycapID, Guid UserID);
}