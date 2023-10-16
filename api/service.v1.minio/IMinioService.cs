using Microsoft.AspNetCore.Http;

namespace service.v1.minio
{
    public interface IMinioService
    {
        public void PushFile(string userID, IFormFile file, string fileType);

        public Task<byte[]> GetFile(string userID, string fileTitle);
    }
}