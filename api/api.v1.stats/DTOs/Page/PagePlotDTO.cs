namespace api.v1.stats.DTOs.Page
{
    public sealed record PagePlotDTO(double LeftDate, double RightDate, Dictionary<Guid, double> Values);
}
