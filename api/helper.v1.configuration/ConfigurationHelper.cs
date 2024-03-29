using Microsoft.Extensions.Configuration;
using helper.v1.configuration.Interfaces;

namespace helper.v1.configuration
{
    public sealed class ConfigurationHelper(IConfiguration cfg) : IEmailConfigurationHelper, IJWTConfigurationHelper, IFileConfigurationHelper, 
        ICacheConfigurationHelper, IActivityConfigurationHelper, IStatConfigurationHelper
    {
        private readonly IConfiguration _cfg = cfg;

        public string GetJWTSecretKey()
        {
            var key = "JWT:SecretKey";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public double GetJWTAccessExpireDate()
        {
            var key = "JWT:AccessExpireDate";
            ValidateConfigurationKey(key, out var str);
            return Convert.ToDouble(str);
        }

        public double GetJWTRefreshExpireDate()
        {
            var key = "JWT:RefreshExpireDate";
            ValidateConfigurationKey(key, out var str);
            return Convert.ToDouble(str);
        }

        public string GetJWTIssuer()
        {
            var key = "JWT:Issuer";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetJWTAudience()
        {
            var key = "JWT:Audience";
            ValidateConfigurationKey(key, out var str);
            return str;
        }



        public string GetEmailHost()
        {
            var key = "Email:Host";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public int GetEmailPort()
        {
            var key = "Email:Port";
            ValidateConfigurationKey(key, out var str);
            return Convert.ToInt32(str);
        }

        public string GetEmailLogin()
        {
            var key = "Email:Login";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetEmailPassword()
        {
            var key = "Email:Password";
            ValidateConfigurationKey(key, out var str);
            return str;
        }



        public Guid GetDefaultModelsUserID()
        {
            var key = "File:DefaultModelsUserID";
            ValidateConfigurationKey(key, out var str);
            return Guid.Parse(str);
        }

        public string GetSwitchFilePath(string fileName)
        {
            var key = "File:SwitchFilePath";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, fileName);
        }

        public string GetSwitchSoundFilePath(string fileName)
        {
            var key = "File:SwitchSoundPath";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, fileName);
        }

        public string GetKeyboardFilePath(Guid userID, string fileName)
        {
            var key = "File:KeyboardFilePath";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, userID, fileName);
        }

        public string GetBoxFilePath(Guid userID, string fileName)
        {
            var key = "File:BoxFilePath";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, userID, fileName);
        }

        public string GetKeycapFilePath(Guid userID, Guid kitID, string fileName)
        {
            var key = "File:KeycapFilePath";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, userID, kitID, fileName);
        }



        public int GetCacheExpirationMinutes()
        {
            var key = "Cache:ExpirationMinutes";
            ValidateConfigurationKey(key, out var str);
            return Convert.ToInt32(str);
        }

        public string GetPaginationListCacheKey(int page, int pageSize, int repositoryFunctionHashCode, Guid param1 = default, Guid param2 = default)
        {
            var key = "Cache:PaginationListCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, page, pageSize, repositoryFunctionHashCode, param1, param2);
        }

        public string GetFileCacheKey(string filePath)
        {
            var key = "Cache:FileCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, filePath);
        }

        public string GetAttendanceTimeCacheKey(double leftDate, double rightDate)
        {
            var key = "Cache:StatisticCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, leftDate, rightDate, "AttendanceTime");
        }

        public string GetAttendanceQuantityCacheKey(double leftDate, double rightDate)
        {
            var key = "Cache:StatisticCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, leftDate, rightDate, "AttendanceQuantity");
        }

        public string GetActivityTimeCacheKey(double leftDate, double rightDate)
        {
            var key = "Cache:StatisticCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, leftDate, rightDate, "ActivityTime");
        }

        public string GetActivityQuantityCacheKey(double leftDate, double rightDate)
        {
            var key = "Cache:StatisticCacheKey";
            ValidateConfigurationKey(key, out var str);
            return string.Format(str, leftDate, rightDate, "ActivityQuantity");
        }



        public string GetSeeKeyboardActivityTag()
        {
            var key = "Activities:SeeKeyboard";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetEditKeyboardActivityTag()
        {
            var key = "Activities:EditKeyboard";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetSeeBoxActivityTag()
        {
            var key = "Activities:SeeBox";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetEditBoxActivityTag()
        {
            var key = "Activities:EditBox";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetSeeKeycapActivityTag()
        {
            var key = "Activities:SeeKeycap";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetEditKeycapActivityTag()
        {
            var key = "Activities:EditKeycap";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public string GetSeeSwitchActivityTag()
        {
            var key = "Activities:SeeSwitch";
            ValidateConfigurationKey(key, out var str);
            return str;
        }

        public int GetStatisticAliveTimeSeconds()
        {
            var key = "Statistic:AliveTimeSeconds";
            ValidateConfigurationKey(key, out var str);
            return Convert.ToInt32(str);
        }



        private void ValidateConfigurationKey(string key, out string result) => result = _cfg[key] ??
            throw new ArgumentNullException($"{key} отсутствует в конфигурационном файле");
    }
}