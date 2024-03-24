﻿using api.v1.main.DTOs;

namespace api.v1.main.Services.Base
{
    public interface IBaseService
    {
        public byte[] GetFile(
            Guid fileID,
            Func<Guid, string?> fileNameFunction,
            Func<string, string> filePathFunction);
        public byte[] GetFile(
            Guid fileID,
            Func<Guid, string?> fileNameFunction,
            Func<Guid, Guid?> userIDFunction,
            Func<Guid, string, string> filePathFunction);



        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Func<int, int, List<Object>> repositoryFunction);
        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid userID,
            Func<int, int, Guid, List<Object>> repositoryFunction);
        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid userID,
            Guid objectTypeID,
            Func<int, int, Guid, Guid, List<Object>> repositoryFunction);



        public int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction);
        public int GetPaginationTotalPages(int pageSize, Guid userID, Func<Guid, int> repositoryFunction);
    }
}