﻿using component.v1.exceptions;

using api.v1.keyboards.DTOs;

using db.v1.keyboards.DTOs.Switch;

namespace api.v1.keyboards.Services.Switch
{
    public interface ISwitchService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetSwitchFileBytes(Guid switchID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetSwitchBase64Sound(Guid switchID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetSwitchBase64Preview(Guid switchID);


        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectSwitchDTO>> GetSwitchesList(PaginationDTO body, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetSwitchesTotalPages(int pageSize);
    }
}