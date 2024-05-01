using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Switch
{
    public interface ISwitchRepository
    {
        public bool IsSwitchExist(Guid switchID);

        public string? SelectSwitchFileName(Guid switchID);

        public List<SelectSwitchDTO> SelectSwitches(int page, int pageSize);
        public int SelectCountOfSwitch();
    }
}