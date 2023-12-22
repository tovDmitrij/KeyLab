namespace api.v1.main.DTOs.File
{
    //public sealed record FilePushDTO(IFormFile File, string Title);
    public sealed record FilePushDTO(IEnumerable<IFormFile> Files, string Title);
}