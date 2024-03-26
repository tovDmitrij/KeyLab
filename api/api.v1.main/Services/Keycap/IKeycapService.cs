using api.v1.main.DTOs;

using db.v1.main.DTOs.Keycap;

namespace api.v1.main.Services.Keycap
{
    public interface IKeycapService
    {
        /// <exception cref="BadRequestException"></exception>
        public byte[] GetKeycapFileBytes(Guid keycapID);
        /// <exception cref="BadRequestException"></exception>
        public string GetKeycapBase64Preview(Guid keycapID);



        /// <exception cref="BadRequestException"></exception>
        public List<SelectKeycapDTO> GetKeycaps(PaginationDTO body, Guid kitID);



        /// <exception cref="BadRequestException"></exception>
        public int GetKeycapsTotalPages(int pageSize, Guid kitID);
    }
}