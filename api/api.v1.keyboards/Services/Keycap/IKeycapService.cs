using api.v1.keyboards.DTOs;
using api.v1.keyboards.DTOs.Keycap;

using db.v1.keyboards.DTOs.Keycap;

using component.v1.exceptions;

namespace api.v1.keyboards.Services.Keycap
{
    public interface IKeycapService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddKeycap(PostKeycapDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeycap(PutKeycapDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task PatchKeycapTitle(PatchKeycapTitleDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetKeycapFileBytes(Guid keycapID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetKeycapBase64Preview(Guid keycapID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeycapDTO>> GetKeycaps(PaginationDTO body, Guid kitID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetKeycapsTotalPages(int pageSize, Guid kitID);
    }
}