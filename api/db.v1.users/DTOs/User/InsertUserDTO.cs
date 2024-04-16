namespace db.v1.users.DTOs.User
{
    public sealed record InsertUserDTO(string Email, string Salt, string HashPass, string Nickname, double RegistrationDate);
}