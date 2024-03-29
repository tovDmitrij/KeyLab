using api.v1.main.DTOs.Keyboard;
using api.v1.main.DTOs;

using db.v1.main.DTOs.Keyboard;

using component.v1.exceptions;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddKeyboard(PostKeyboardDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeyboard(PutKeyboardDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task DeleteKeyboard(DeleteKeyboardDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetKeyboardFileBytes(Guid keyboardID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetKeyboardBase64Preview(Guid keyboardID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeyboardDTO>> GetDefaultKeyboardsList(PaginationDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKeyboardDTO>> GetUserKeyboardsList(PaginationDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultKeyboardsTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize);
    }
}