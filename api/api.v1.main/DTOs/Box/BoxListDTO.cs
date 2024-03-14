namespace api.v1.main.DTOs.Box
{
    public sealed record BoxListDTO(Guid ID, Guid TypeID, string TypeTitle, string Title, 
                                    string? Description, string Preview, double CreationDate);
}