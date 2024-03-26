using api.v1.main.DTOs;

namespace api.v1.main.Services.BaseAlgorithm
{
    public interface IBaseAlgorithmService
    {
        public InitFileDTO AddFile(
            IFormFile? file,
            IFormFile? preview,
            Guid userID,
            string title,
            Func<Guid, string, string> filePathFunction);
        public InitFileDTO UpdateFile(
            IFormFile? file,
            IFormFile? preview,
            Guid userID,
            Guid objectID,
            string title,
            Func<Guid, string> fileNameFunction,
            Func<Guid, string> previewNameFunction,
            Func<Guid, string, string> filePathFunction);



        public byte[] GetFile(
            Guid param1,
            Func<Guid, string?> fileNameFunction,
            Func<string, string> filePathFunction);
        public byte[] GetFile(
            Guid param1,
            Func<Guid, string?> fileNameFunction,
            Func<Guid, Guid?> userIDFunction,
            Func<Guid, string, string> filePathFunction);
        public byte[] GetFile(
            Guid param1,
            Guid param2,
            Func<Guid, string?> fileNameFunction,
            Func<Guid, Guid?> userIDFunction,
            Func<Guid, Guid, string, string> filePathFunction);



        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Func<int, int, List<Object>> repositoryFunction);
        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Func<int, int, Guid, List<Object>> repositoryFunction);
        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Guid param2,
            Func<int, int, Guid, Guid, List<Object>> repositoryFunction);



        public int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction);
        public int GetPaginationTotalPages(int pageSize, Guid param, Func<Guid, int> repositoryFunction);
    }
}