namespace lake_data_api.Models;
public class LakeWaterLevel {
    public string? Id { get; set; }
    public decimal SurfaceElevation { get; set; }
    public DateTime WaterLevelTimeStamp { get; set; }
    public required string LakeId { get; set; }
}