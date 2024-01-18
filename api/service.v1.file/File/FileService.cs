using service.v1.configuration.Interfaces;

namespace service.v1.file.File
{
    public sealed class FileService : IFileService
    {
        private readonly IFileConfigurationService _cfg;

        public FileService(IFileConfigurationService cfg)
        {
            _cfg = cfg;
        }



        public void AddFile(byte[] file, string filePath)
        {
            var parentDirectory = _cfg.GetModelsDirectoryPath();
            var fullFilePath = Path.Combine(parentDirectory, filePath);
            Directory.CreateDirectory(fullFilePath[..fullFilePath.LastIndexOf('/')]);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(file, 0, file.Length);
            }
        }

        public byte[] GetFile(string filePath)
        {
            var parentDirectory = _cfg.GetModelsDirectoryPath();
            var fullFilePath = Path.Combine(parentDirectory, filePath);

            return System.IO.File.ReadAllBytes(fullFilePath);
        }

        public bool IsFileExist(string filePath)
        {
            var parentDirectory = _cfg.GetModelsDirectoryPath();
            var fullFilePath = Path.Combine(parentDirectory, filePath);

            return System.IO.File.Exists(fullFilePath);
        }
    }
}