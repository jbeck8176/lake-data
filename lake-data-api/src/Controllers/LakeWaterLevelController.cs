using Microsoft.AspNetCore.Mvc;
using lake_data_api.Services;

namespace lake_data_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LakeWaterLevelController : ControllerBase
{
    private readonly ILogger<LakeController> _logger;
    private readonly ILakeWaterLevelService _lakeWaterLevelService; 
    private readonly ILakeRepository _lakeRepository;
    private readonly ILakeWaterLevelRepository _lakeWaterLevelRepository;

    public LakeWaterLevelController(ILogger<LakeController> logger, 
    ILakeWaterLevelService lakeWaterLevelService,
    ILakeRepository lakeRepository,
    ILakeWaterLevelRepository lakeWaterLevelRepository)
    {
        _logger = logger;
        _lakeWaterLevelService = lakeWaterLevelService;
        _lakeRepository = lakeRepository;
        _lakeWaterLevelRepository = lakeWaterLevelRepository;
    }

    [HttpGet("{lakeId}")]
    public async Task<IActionResult> ProcessLakeData(string lakeId)
    {
        try
        {
            if (string.IsNullOrEmpty(lakeId))
            {
                return BadRequest("Lake ID cannot be null or empty");
            }

            var lake = await _lakeRepository.FindById(lakeId);
            if (lake == null)
            {
                return NotFound("Lake not found");
            }
            
            var lakeWaterLevel = await _lakeWaterLevelService.GetLakeWaterLevel(lake);
            if (lakeWaterLevel == null)
            {
                return NotFound("Lake water level not found");
            }

            lakeWaterLevel = await _lakeWaterLevelRepository.Create(lakeWaterLevel);
            
            return Ok(lakeWaterLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing lake data");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
