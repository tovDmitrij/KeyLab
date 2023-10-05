namespace db.v1.main.Entities.Users
{
    public sealed class UserSecurityEntity
    {
        public Guid ID { get; set; }
        public string Salt { get; set; }
        public UserSecurityEntity(Guid id, string salt) 
        {
            ID = id;
            Salt = salt;
        }
    }
}