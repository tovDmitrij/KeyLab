namespace helper.v1.file
{
    public sealed class FileHelper : IFileHelper
    {
        public async Task UploadFileAsync(byte[] file, string fullFilePath)
        {
            Directory.CreateDirectory(fullFilePath[..fullFilePath.LastIndexOf('/')]);

            using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
            await fileStream.WriteAsync(file);
        }

        public void DeleteFile(string fullFilePath) => File.Delete(fullFilePath);

        public async Task<byte[]> ReadFileAsync(string fullFilePath)
        {
            try
            {
                return await File.ReadAllBytesAsync(fullFilePath);
            }
            catch
            {
                return [];
            }
        }
    }
}