namespace api.v1.main.DTOs.Box
{
    public sealed record PostBoxDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid TypeID, Guid UserID);
}