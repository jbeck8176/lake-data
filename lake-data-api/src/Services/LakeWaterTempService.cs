using lake_data_api.Models;
using System.Text.Json.Nodes;

namespace lake_data_api.Services;

public interface ILakeWaterTempService
{
    Task<LakeWaterTemp> GetLakeWaterTemp(Lake lake);
}

public class USGSLakeWaterTempService: ILakeWaterTempService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<USGSLakeWaterTempService> _logger;

     public USGSLakeWaterTempService(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<USGSLakeWaterTempService> logger)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
        _logger = logger;
    }

      public async Task<LakeWaterTemp> GetLakeWaterTemp(Lake lake)
    {
        if(lake == null || string.IsNullOrEmpty(lake.Id))
        {
            throw new ArgumentNullException(nameof(lake), "Lake cannot be null");
        }

        if(string.IsNullOrEmpty(lake.WQDataSiteId.ToString()))
        {
            throw new ArgumentNullException(nameof(lake.WQDataSiteId), "Lake USGS Site ID cannot be null");
        }

        try
        {
            using var httpClient = _clientFactory.CreateClient();
            // WG Data call
            var data = new Dictionary<string, string>()
                    {
                        {"siteID", lake.WQDataSiteId.ToString() }
                    };
            var encodedContent = new FormUrlEncodedContent(data);

            var response = await httpClient.PostAsync("https://www.wqdatalive.com/public/15/sidebar", encodedContent);
            
            if(!response.IsSuccessStatusCode) {
                _logger.LogError("Error getting lake water temp for {WQDataSiteId}: {StatusCode}", lake.WQDataSiteId, response.StatusCode);
                throw new Exception($"Error getting lake water temp for {lake.WQDataSiteId}: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            var waterTemp = new LakeWaterTemp
            {
                LakeId = lake.Id,
                TempInFahrenheit = decimal.Parse(json?["data"]?[0]?[0]?["value"]?.ToString()??"0"),
                WaterTempTimeStamp = DateTime.Parse(json?["latestTimestamp"]?.ToString()??DateTime.Now.ToString())
            };

            waterTemp.TempInCelsius = (waterTemp.TempInFahrenheit - 32) * ((decimal)5/(decimal)9);

            return waterTemp;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lake water level for {USGSSiteId}", lake.USGSSiteId);
            throw new Exception($"Error getting lake water level for {lake.USGSSiteId}", ex);
        }

    }

}