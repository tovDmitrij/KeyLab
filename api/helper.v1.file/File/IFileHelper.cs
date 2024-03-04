namespace helper.v1.file.File
{
    public interface IFileHelper
    {
        public void AddFile(byte[] file, string fullFilePath);
        public void UpdateFile(byte[] file, string fullFilePath);
        public void DeleteFile(string fullFilePath);
        public byte[] GetFile(string fullFilePath);
        public bool IsFileExist(string fullFilePath);
    }
}