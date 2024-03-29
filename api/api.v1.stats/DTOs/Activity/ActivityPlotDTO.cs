namespace api.v1.stats.DTOs.Activity
{
    public sealed record ActivityPlotDTO(double LeftDate, double RightDate, Dictionary<Guid, double> Values);
}
