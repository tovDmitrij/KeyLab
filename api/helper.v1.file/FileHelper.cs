namespace helper.v1.file
{
    public sealed class FileHelper : IFileHelper
    {
        public void AddFile(byte[] file, string fullFilePath)
        {
            Directory.CreateDirectory(fullFilePath[..fullFilePath.LastIndexOf('/')]);

            using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
            fileStream.Write(file, 0, file.Length);
        }

        public void DeleteFile(string fullFilePath) => File.Delete(fullFilePath);

        public byte[] GetFile(string fullFilePath)
        {
            try
            {
                return File.ReadAllBytes(fullFilePath);
            }
            catch
            {
                return [];
            }
        }
    }
}