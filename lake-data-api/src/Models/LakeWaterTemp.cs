namespace lake_data_api.Models;
public class LakeWaterTemp {
    public string? Id { get; set; }
    public decimal TempInFahrenheit { get; set; }
    public decimal TempInCelsius  { get; set; }
    public DateTime WaterTempTimeStamp { get; set; }
    public required string LakeId { get; set; }
}