namespace helper.v1.configuration.Interfaces
{
    public interface ICacheConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public int GetCacheExpirationMinutes();



        /// <exception cref="ArgumentNullException"></exception>
        public string GetPaginationListCacheKey(int page, int pageSize, int repositoryFunctionHashCode, Guid param1 = default, Guid param2 = default);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetFileCacheKey(string filePath);



        /// <exception cref="ArgumentNullException"></exception>
        public string GetAttendanceTimeCacheKey(double leftDate, double rightDate);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetAttendanceQuantityCacheKey(double leftDate, double rightDate);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetActivityTimeCacheKey(double leftDate, double rightDate);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetActivityQuantityCacheKey(double leftDate, double rightDate);
    }
}