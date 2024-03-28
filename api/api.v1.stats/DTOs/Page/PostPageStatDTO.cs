namespace api.v1.stats.DTOs.Page
{
    public sealed record PostPageStatDTO(double LeftDate, double RightDate, Guid IntervalID, Guid[] PageIDs);
}