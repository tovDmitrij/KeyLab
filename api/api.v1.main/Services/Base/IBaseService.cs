using api.v1.main.DTOs;

namespace api.v1.main.Services.Base
{
    public interface IBaseService
    {
        public int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction);
        public int GetPaginationTotalPages(int pageSize, Guid userID, Func<Guid, int> repositoryFunction);
    }
}