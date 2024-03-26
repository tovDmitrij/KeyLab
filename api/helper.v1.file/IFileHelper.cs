namespace helper.v1.file
{
    public interface IFileHelper
    {
        public void AddFile(byte[] file, string fullFilePath);
        public void DeleteFile(string fullFilePath);
        public byte[] GetFile(string fullFilePath);
    }
}