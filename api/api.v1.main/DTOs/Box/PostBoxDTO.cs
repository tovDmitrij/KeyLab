namespace api.v1.main.DTOs.Box
{
    public sealed record PostBoxDTO(IFormFile? File, string Title, string? Description, Guid TypeID, Guid UserID);
}