namespace service.v1.file.File
{
    public interface IFileService
    {
        public void AddFile(byte[] file, string filePath, string fileName);

        public Task<byte[]> GetFile(string filePath);
    }
}