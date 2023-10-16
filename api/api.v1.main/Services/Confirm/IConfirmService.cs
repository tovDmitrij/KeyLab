using api.v1.main.DTOs.User;

namespace api.v1.main.Services.Confirm
{
    public interface IConfirmService
    {
        public void ConfirmEmail(ConfirmEmailDTO body);
    }
}