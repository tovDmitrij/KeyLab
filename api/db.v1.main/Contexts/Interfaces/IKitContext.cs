﻿using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IKitContext
    {
        public DbSet<KitEntity> Kits { get; set; }
        public int SaveChanges();
    }
}