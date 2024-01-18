﻿namespace service.v1.file.File
{
    public interface IFileService
    {
        public void AddFile(byte[] file, string filePath);
        public byte[] GetFile(string filePath);
        public bool IsFileExist(string filePath);
    }
}