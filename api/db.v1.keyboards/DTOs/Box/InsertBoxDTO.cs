﻿namespace db.v1.keyboards.DTOs.Box
{
    public record InsertBoxDTO(Guid OwnerID, Guid BoxTypeID, string Title, string FileName, string PreviewName, double CreationDate);
}