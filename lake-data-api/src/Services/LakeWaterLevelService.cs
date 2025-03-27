using lake_data_api.Models;
using System.Text.Json.Nodes;
using System.Xml.Linq;


namespace lake_data_api.Services;

public interface ILakeWaterLevelService
{
    Task<LakeWaterLevelModel> GetLakeWaterLevel(Lake lake);
}

public class USGSLakeWaterLevelService(IHttpClientFactory clientFactory): ILakeWaterLevelService
{
    public async Task<LakeWaterLevelModel> GetLakeWaterLevel(Lake lake)
    {
        var waterLevel = new LakeWaterLevelModel
        {
            LakeWaterLevel = 0,
            TimeStamp = DateTime.Now
        };

        try
        {
            using var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://waterservices.usgs.gov/nwis/iv?format=json&site={lake.USGSSiteId}&agencyCd=USGS");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
   
                var json = JsonObject.Parse(content);

                waterLevel.LakeWaterLevel = decimal.Parse(json?["value"]?["timeSeries"]?[0]?["values"]?[0]?["value"]?[0]?["value"]?.ToString()??"0");
                waterLevel.TimeStamp = DateTime.Parse(json?["value"]?["timeSeries"]?[0]?["values"]?[0]?["value"]?[0]?["dateTime"]?.ToString()??DateTime.Now.ToString());

                return waterLevel;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return waterLevel;
    }

}
