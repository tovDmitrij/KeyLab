namespace helper.v1.file
{
    public interface IFileHelper
    {
        public Task UploadFileAsync(byte[] file, string fullFilePath);
        public void DeleteFile(string fullFilePath);
        public Task<byte[]> ReadFileAsync(string fullFilePath);
    }
}