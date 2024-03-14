namespace db.v1.main.DTOs.Box
{
    public record InsertBoxDTO(Guid OwnerID, Guid BoxTypeID, string Title, string? Description, string FileName, 
                               string PreviewName, double CreationDate);
}