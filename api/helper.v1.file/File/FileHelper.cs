namespace helper.v1.file.File
{
    public sealed class FileHelper : IFileHelper
    {
        public void AddFile(byte[] file, string fullFilePath)
        {
            Directory.CreateDirectory(fullFilePath[..fullFilePath.LastIndexOf('/')]);

            using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
            fileStream.Write(file, 0, file.Length);
        }

        public void UpdateFile(byte[] file, string fullFilePath)
        {
            using var fileStream = new FileStream(fullFilePath, FileMode.CreateNew, FileAccess.Write);
            fileStream.Write(file, 0, file.Length);
        }

        public void DeleteFile(string fullFilePath) => System.IO.File.Delete(fullFilePath);

        public byte[] GetFile(string fullFilePath) => System.IO.File.ReadAllBytes(fullFilePath);

        public bool IsFileExist(string fullFilePath) => System.IO.File.Exists(fullFilePath);
    }
}