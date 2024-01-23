using db.v1.main.Contexts.Interfaces;

namespace db.v1.main.Repositories.Switch
{
    public sealed class SwitchRepository : ISwitchRepository
    {
        private readonly ISwitchContext _db;

        public SwitchRepository(ISwitchContext db) => _db = db;



        public bool IsSwitchExist(Guid switchID) => _db.Switches
            .Any(x => x.ID == switchID);
    }
}