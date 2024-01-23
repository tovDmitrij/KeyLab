namespace db.v1.main.Repositories.Box
{
    public interface IBoxRepository
    {
        public bool IsBoxTypeExist(Guid boxTypeID);
    }
}