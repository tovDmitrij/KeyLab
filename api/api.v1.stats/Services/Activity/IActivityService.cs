﻿using db.v1.stats.DTOs;

namespace api.v1.stats.Services.Activity
{
    public interface IActivityService
    {
        public List<SelectActivityDTO> GetActivities();
    }
}