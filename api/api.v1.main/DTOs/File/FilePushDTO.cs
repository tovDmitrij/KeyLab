namespace api.v1.main.DTOs.File
{
    public sealed record FilePushDTO(IFormFile File, string FileType);
}