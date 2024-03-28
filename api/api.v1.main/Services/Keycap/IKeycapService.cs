using api.v1.main.DTOs;

using db.v1.main.DTOs.Keycap;

using component.v1.exceptions;
using api.v1.main.DTOs.Keycap;

namespace api.v1.main.Services.Keycap
{
    public interface IKeycapService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddKeycap(PostKeycapDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeycap(PutKeycapDTO body, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetKeycapFileBytes(Guid keycapID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public string GetKeycapBase64Preview(Guid keycapID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeycapDTO>> GetKeycaps(PaginationDTO body, Guid kitID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetKeycapsTotalPages(int pageSize, Guid kitID);
    }
}