namespace api.v1.keyboards.DTOs.Box
{
    public sealed record PutBoxDTO(IFormFile? File, IFormFile? Preview, string? Title, Guid UserID, Guid BoxID);
}