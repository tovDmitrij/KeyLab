using api.v1.keyboards.DTOs.Keycap;

using db.v1.keyboards.DTOs;

using component.v1.exceptions;

namespace api.v1.keyboards.Services.Keycap
{
    public interface IKeycapService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeycap(IFormFile? file, Guid keycapID, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetKeycapFileBytes(Guid keycapID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeycapDTO>> GetKeycaps(int page, int pageSize, Guid kitID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetKeycapsTotalPages(int pageSize, Guid kitID);
    }
}