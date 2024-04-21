namespace helper.v1.file
{
    public interface IFileHelper
    {
        public Task UploadFileAsync(byte[] file, string fullFilePath);
        public void CopyFile(string sourceFilePath, string destinationFilePath);
        public void DeleteFile(string fullFilePath);
        public Task<byte[]> ReadFileAsync(string fullFilePath);
    }
}