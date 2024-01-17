namespace service.v1.file.Object
{
    public interface IObjectFileService
    {
        public void AddFile(string bucketName, byte[] file);

        public Task<byte[]> GetFile(string bucketName, string fileTitle);
    }
}