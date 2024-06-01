using api.v1.keyboards.DTOs.Box;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;

namespace api.v1.keyboards.Services.Box
{
    public interface IBoxService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<Guid> AddBox(IFormFile? file, IFormFile? preview, string? title, Guid typeID, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateBox(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid boxID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task DeleteBox(DeleteBoxDTO body, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task PatchBoxTitle(PatchBoxTitleDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetBoxFileBytes(Guid boxID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetBoxBase64Preview(Guid boxID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectBoxDTO>> GetDefaultBoxesList(int page, int pageSize, Guid boxTypeID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectBoxDTO>> GetUserBoxesList(int page, int pageSize, Guid boxTypeID, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultBoxesTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserBoxesTotalPages(Guid userID, int pageSize);



        public List<SelectBoxTypeDTO> GetBoxTypes();
    }
}