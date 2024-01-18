namespace service.v1.file.File
{
    public sealed class FileService : IFileService
    {
        public void AddFile(byte[] file, string filePath, string fileName)
        {
            Directory.CreateDirectory(filePath);

            var fullFilePath = Path.Combine(filePath, fileName);
            using (var fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(file, 0, file.Length);
            }
        }

        public byte[] GetFile(string filePath) => System.IO.File.ReadAllBytes(filePath);

        public bool IsFileExist(string filePath) => System.IO.File.Exists(filePath);
    }
}