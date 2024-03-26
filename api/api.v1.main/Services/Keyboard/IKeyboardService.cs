using api.v1.main.DTOs.Keyboard;
using api.v1.main.DTOs;

using component.v1.exceptions;
using db.v1.main.DTOs.Keyboard;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        /// <exception cref="BadRequestException"></exception>
        public void AddKeyboard(PostKeyboardDTO body);
        /// <exception cref="BadRequestException"></exception>
        public void UpdateKeyboard(PutKeyboardDTO body);
        /// <exception cref="BadRequestException"></exception>
        public void DeleteKeyboard(DeleteKeyboardDTO body, Guid userID);



        /// <exception cref="BadRequestException"></exception>
        public byte[] GetKeyboardFileBytes(Guid keyboardID);
        /// <exception cref="BadRequestException"></exception>
        public string GetKeyboardBase64Preview(Guid keyboardID);



        /// <exception cref="BadRequestException"></exception>
        public List<SelectKeyboardDTO> GetDefaultKeyboardsList(PaginationDTO body);
        /// <exception cref="BadRequestException"></exception>
        public List<SelectKeyboardDTO> GetUserKeyboardsList(PaginationDTO body, Guid userID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultKeyboardsTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize);
    }
}