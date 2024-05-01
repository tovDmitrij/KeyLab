using api.v1.keyboards.DTOs.Keyboard;

using db.v1.keyboards.DTOs;

using component.v1.exceptions;

namespace api.v1.keyboards.Services.Keyboard
{
    public interface IKeyboardService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddKeyboard(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid boxTypeID, Guid switchTypeID, 
            Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeyboard(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid keyboardID, 
            Guid boxTypeID, Guid switchTypeID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task DeleteKeyboard(DeleteKeyboardDTO body, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task PatchKeyboardTitle(PatchKeyboardTitleDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetKeyboardFileBytes(Guid keyboardID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetKeyboardBase64Preview(Guid keyboardID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeyboardDTO>> GetDefaultKeyboardsList(int page, int pageSize, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeyboardDTO>> GetUserKeyboardsList(int page, int pageSize, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultKeyboardsTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize);
    }
}