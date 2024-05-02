namespace api.v1.stats.DTOs.Activity
{
    public sealed record ActivityTimePlotDTO(double LeftDate, double RightDate, Dictionary<Guid, double> Values);
}
