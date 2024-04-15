﻿namespace component.v1.user
{
    public sealed record UserDTO(string Email, string Salt, string HashPass, string Nickname, double RegistrationDate);
}