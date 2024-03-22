namespace api.v1.main.DTOs.Box
{
    public sealed record PutBoxDTO(IFormFile? File, string Title, Guid UserID, Guid BoxID);
}