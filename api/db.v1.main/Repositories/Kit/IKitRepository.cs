﻿using db.v1.main.DTOs.Kit;

namespace db.v1.main.Repositories.Kit
{
    public interface IKitRepository
    {
        public Guid? SelectKitOwnerID(Guid kitID);

        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID);

        public int SelectCountOfKits(Guid userID);

        public bool IsKitExist(Guid kitID);
    }
}