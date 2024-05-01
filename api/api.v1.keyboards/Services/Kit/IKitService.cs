using api.v1.keyboards.DTOs.Kit;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;

namespace api.v1.keyboards.Services.Kit
{
    public interface IKitService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<Guid> CreateKit(IFormFile? preview, Guid boxTypeID, string? title, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task PatchKitTitle(Guid kitID, string title, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task PatchKitPreview(IFormFile? preview, Guid kitID, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task DeleteKit(DeleteKitDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetKitPreviewBase64(Guid kitID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKitDTO>> GetDefaultKits(int page, int pageSize, Guid boxTypeID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKitDTO>> GetUserKits(int page, int pageSize, Guid boxTypeID, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultKitsTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserKitsTotalPages(int pageSize, Guid userID);
    }
}