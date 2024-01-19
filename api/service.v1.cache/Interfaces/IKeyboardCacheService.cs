using db.v1.main.DTOs;

namespace service.v1.cache
{
    public interface IKeyboardCacheService
    {
        public bool TryGetDefaultKeyboardsList(out List<KeyboardDTO>? keyboards);
        public void SetDefaultKeyboardsList(List<KeyboardDTO> keyboards);

        public bool TryGetKeyboardFile(Guid keyboardID, out byte[]? keyboard);
        public void SetKeyboardFile(Guid keyboardID, byte[] keyboard);
    }
}