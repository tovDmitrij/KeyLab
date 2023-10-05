using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities.Confirm
{
    [Table("email_confirms")]
    public sealed class EmailConfirmEntity
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("expire_date")]
        public double ExpireDate { get; set; }

        public EmailConfirmEntity(long id, string email, string code, double expireDate)
        {
            ID = id;
            Email = email;
            Code = code;
            ExpireDate = expireDate;
        }

        public EmailConfirmEntity(string email, string code, double expireDate)
        {
            Email = email;
            Code = code;
            ExpireDate = expireDate;
        }

        public EmailConfirmEntity() { }
    }
}