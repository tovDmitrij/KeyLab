namespace api.v1.keyboards.DTOs.Box
{
    public sealed record BoxPaginationDTO(int Page, int PageSize, Guid TypeID);
}