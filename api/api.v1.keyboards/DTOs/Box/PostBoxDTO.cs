namespace api.v1.keyboards.DTOs.Box
{
    public sealed record PostBoxDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid TypeID, Guid UserID);
}