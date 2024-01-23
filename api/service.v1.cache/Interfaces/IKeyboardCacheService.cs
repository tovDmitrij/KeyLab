using db.v1.main.DTOs;

namespace service.v1.cache
{
    public interface IKeyboardCacheService
    {
        public bool TryGetKeyboardsList(Guid userID, out List<KeyboardInfoDTO>? keyboards);
        public void SetKeyboardsList(Guid userID, List<KeyboardInfoDTO> keyboards);
        public void DeleteKeyboardsList(Guid userID);

        public bool TryGetFile(Guid fileID, out byte[]? file);
        public void SetFile(Guid fileID, byte[] file);
        public void DeleteFile(Guid fileID);

    }
}