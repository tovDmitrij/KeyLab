using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities
{
    [Table("email_codes")]
    public sealed class EmailVerificationCodeEntity
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

        public EmailVerificationCodeEntity(long id, string email, string code, double expireDate)
        {
            ID = id;
            Email = email;
            Code = code;
            ExpireDate = expireDate;
        }

        public EmailVerificationCodeEntity(string email, string code, double expireDate)
        {
            Email = email;
            Code = code;
            ExpireDate = expireDate;
        }

        public EmailVerificationCodeEntity() { }
    }
}