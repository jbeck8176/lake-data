using Microsoft.AspNetCore.Mvc;
using lake_data_api.Services;
using lake_data_api.Models;

namespace lake_data_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LakeDataController : ControllerBase
{

    private readonly ILogger<LakeDataController> _logger;
    private readonly ILakeWaterLevelService _lakeWaterLevelService;

    public LakeDataController(ILogger<LakeDataController> logger, ILakeWaterLevelService lakeWaterLevelService)
    {
        _logger = logger;
        _lakeWaterLevelService = lakeWaterLevelService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var lake = new Lake
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                USGSSiteId = id,
                WQDataSiteId = 55,
                Latitude = 39.0968,
                Longitude = -120.0324
            };

            var lakeWaterLevel = await _lakeWaterLevelService.GetLakeWaterLevel(lake);

            lake.LakeWaterLevel = lakeWaterLevel;

            return Ok(lake);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lake data for {id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
