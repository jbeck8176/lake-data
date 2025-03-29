namespace lake_data_api.Models;

public class Lake
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required string USGSSiteId { get; set; }
    public required int WQDataSiteId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public LakeWaterLevelModel? LakeWaterLevel { get; set; }
}
