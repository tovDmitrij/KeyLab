using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Switch
{
    public sealed class SwitchRepository(ISwitchContext db) : ISwitchRepository
    {
        private readonly ISwitchContext _db = db;

        public bool IsSwitchExist(Guid switchID) => _db.Switches.Any(x => x.ID == switchID);

        

        public string? SelectSwitchFileName(Guid switchID) => _db.Switches.FirstOrDefault(x => x.ID == switchID)?.FileName;



        public List<SelectSwitchDTO> SelectSwitches(int page, int pageSize) => _db.Switches
            .Select(x => new SelectSwitchDTO(x.ID, x.Title)).Skip((page - 1) * pageSize).Take(pageSize).ToList();

        public int SelectCountOfSwitch() => _db.Switches.Count();
    }
}