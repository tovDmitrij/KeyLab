namespace helper.v1.cache
{
    public interface ICacheHelper
    {
        public bool TryGetValue<T>(object key, out T? value);
        public void DeleteValue(object key);
        public void SetValue<T>(object key, T value, int minutes);
    }
}