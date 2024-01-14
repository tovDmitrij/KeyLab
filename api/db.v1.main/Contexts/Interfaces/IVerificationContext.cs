﻿using db.v1.main.Entities.EmailVerificationCode;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IVerificationContext
    {
        public DbSet<EmailVerificationCodeEntity> EmailCodes { get; set; }
        public int SaveChanges();
    }
}