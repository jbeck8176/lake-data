using lake_data_api.Models;
using System.Text.Json.Nodes;


namespace lake_data_api.Services;

public interface ILakeWaterLevelService
{
    Task<LakeWaterLevel> GetLakeWaterLevel(Lake lake);
}

public class USGSLakeWaterLevelService: ILakeWaterLevelService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<USGSLakeWaterLevelService> _logger;

    public USGSLakeWaterLevelService(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<USGSLakeWaterLevelService> logger)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<LakeWaterLevel> GetLakeWaterLevel(Lake lake)
    {
        if(lake == null || string.IsNullOrEmpty(lake.Id))
        {
            throw new ArgumentNullException(nameof(lake), "Lake cannot be null");
        }

        if(string.IsNullOrEmpty(lake.USGSSiteId))
        {
            throw new ArgumentNullException(nameof(lake.USGSSiteId), "Lake USGS Site ID cannot be null");
        }

        try
        {
            using var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_configuration["USGSUrl"]}?format=json&site={lake.USGSSiteId}&agencyCd=USGS");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error getting lake water level for {USGSSiteId}: {StatusCode}", lake.USGSSiteId, response.StatusCode);
                throw new Exception($"Error getting lake water level for {lake.USGSSiteId}: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
   
            var json = JsonNode.Parse(content);

            var waterLevel = new LakeWaterLevel
            {
                LakeId = lake.Id,
                SurfaceElevation = decimal.Parse(json?["value"]?["timeSeries"]?[0]?["values"]?[0]?["value"]?[0]?["value"]?.ToString()??"0"),
                WaterLevelTimeStamp = DateTime.Parse(json?["value"]?["timeSeries"]?[0]?["values"]?[0]?["value"]?[0]?["dateTime"]?.ToString()??DateTime.Now.ToString())
            };

            return waterLevel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lake water level for {USGSSiteId}", lake.USGSSiteId);
            throw new Exception($"Error getting lake water level for {lake.USGSSiteId}", ex);
        }
    }

}
