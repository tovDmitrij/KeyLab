namespace api.v1.main.DTOs.Box
{
    public sealed record BoxPaginationDTO(int Page, int PageSize, Guid TypeID);
}