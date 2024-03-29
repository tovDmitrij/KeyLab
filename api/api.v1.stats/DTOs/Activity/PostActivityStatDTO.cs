namespace api.v1.stats.DTOs.Activity
{
    public sealed record PostActivityStatDTO(double LeftDate, double RightDate, Guid IntervalID, Guid[] ActivityIDs);
}