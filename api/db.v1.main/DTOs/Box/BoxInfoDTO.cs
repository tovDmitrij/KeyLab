﻿namespace db.v1.main.DTOs.Box
{
    public sealed record BoxInfoDTO(
        Guid ID, Guid TypeID, string TypeTitle,
        string Title, string? Description, double CreationDate);
}