using db.v1.main.DTOs;

namespace service.v1.cache
{
    public interface IKeyboardCacheService
    {
        public bool TryGetKeyboardsList(Guid key, out List<KeyboardDTO>? keyboards);
        public void SetKeyboardsList(Guid key, List<KeyboardDTO> keyboards);

        public bool TryGetFile(Guid key, out byte[]? keyboard);
        public void SetFile(Guid key, byte[] keyboard);
    }
}