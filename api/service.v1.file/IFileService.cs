using Microsoft.AspNetCore.Http;

namespace service.v1.file
{
    public interface IFileService
    {
        public void PushFile(string userID, IFormFile file);

        public Task<byte[]> GetFile(string userID, string fileTitle);
    }
}