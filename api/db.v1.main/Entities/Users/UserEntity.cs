using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities.Users
{
    [Table("users")]
    public sealed class UserEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [Column("token_expire_date")]
        public double? TokenExpireDate { get; set; }

        [Column("registration_date")]
        public double RegistrationDate { get; set; }

        public UserEntity(Guid id, string email, string salt, string password, string nickname, string? token, double? tokenExpireDate, double registrationDate)
        {
            ID = id;
            Email = email;
            Salt = salt;
            Password = password;
            Nickname = nickname;
            Token = token;
            TokenExpireDate = tokenExpireDate;
            RegistrationDate = registrationDate;
        }

        public UserEntity(string email, string salt, string password, string nickname)
        {
            Email = email;
            Salt = salt;
            Password = password;
            Nickname = nickname;
        }

        public UserEntity() { }
    }
}