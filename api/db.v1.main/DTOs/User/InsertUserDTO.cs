namespace db.v1.main.DTOs.User
{
    public sealed record InsertUserDTO(string Email, string Salt, string HashPass, 
                                   string Nickname, double RegistrationDate);
}