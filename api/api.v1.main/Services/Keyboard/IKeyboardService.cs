using api.v1.main.DTOs.Keyboard;
using api.v1.main.DTOs;

using component.v1.exceptions;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddKeyboard(PostKeyboardDTO body);

        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKeyboard(PutKeyboardDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void DeleteKeyboard(DeleteKeyboardDTO body);

        /// <exception cref="BadRequestException"></exception>
        public byte[] GetKeyboardFile(Guid keyboardID);

        /// <exception cref="BadRequestException"></exception>
        public List<KeyboardListDTO> GetDefaultKeyboardsList(PaginationDTO body);

        public int GetDefaultKeyboardsTotalPages(int pageSize);

        /// <exception cref="BadRequestException"></exception>
        public List<KeyboardListDTO> GetUserKeyboardsList(PaginationDTO body, Guid userID);

        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize);

    }
}