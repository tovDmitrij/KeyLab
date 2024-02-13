namespace api.v1.main.Services.Profile
{
    public interface IProfileService
    {
        public string GetUserNickname(Guid userID);
    }
}