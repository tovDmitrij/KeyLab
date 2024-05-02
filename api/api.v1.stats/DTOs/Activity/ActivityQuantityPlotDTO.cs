namespace api.v1.stats.DTOs.Activity
{
    public sealed record ActivityQuantityPlotDTO(double LeftDate, double RightDate, Dictionary<Guid, int> Values);
}