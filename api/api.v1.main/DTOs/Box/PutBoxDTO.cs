namespace api.v1.main.DTOs.Box
{
    public sealed record PutBoxDTO(IFormFile? File, string Title, string? Description, Guid UserID, Guid BoxID);
}