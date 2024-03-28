namespace helper.v1.configuration.Interfaces
{
    public interface IActivityConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public string GetRefreshActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSeeKeyboardActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetEditKeyboardActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSeeBoxActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetEditBoxActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSeeKeycapActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetEditKeycapActivityTag();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSeeSwitchActivityTag();
    }
}