namespace service.v1.file.File
{
    public sealed class FileService : IFileService
    {
        public void AddFile(byte[] file, string filePath)
        {
            Directory.CreateDirectory(filePath[..filePath.LastIndexOf('/')]);

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fileStream.Write(file, 0, file.Length);
        }

        public byte[] GetFile(string filePath) => System.IO.File.ReadAllBytes(filePath);

        public bool IsFileExist(string filePath) => System.IO.File.Exists(filePath);
    }
}