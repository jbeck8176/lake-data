using Microsoft.AspNetCore.Mvc;
using lake_data_api.Services;

namespace lake_data_api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LakeWaterTempController : ControllerBase
{
    private readonly ILogger<LakeController> _logger;
    private readonly ILakeWaterTempService _lakeWaterTempService; 
    private readonly ILakeRepository _lakeRepository;
    private readonly ILakeWaterTempRepository _lakeWaterTempRepository;

    public LakeWaterTempController(ILogger<LakeController> logger, 
    ILakeWaterTempService lakeWaterTempService,
    ILakeRepository lakeRepository,
    ILakeWaterTempRepository lakeWaterTempRepository)
    {
        _logger = logger;
        _lakeWaterTempService = lakeWaterTempService;
        _lakeRepository = lakeRepository;
        _lakeWaterTempRepository = lakeWaterTempRepository;
    }

     [HttpGet("{lakeId}")]
    public async Task<IActionResult> ProcessLakeTempData(string lakeId)
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
            
            var lakeWaterTemp = await _lakeWaterTempService.GetLakeWaterTemp(lake);
            if (lakeWaterTemp == null)
            {
                return NotFound("Lake water temperature not found");
            }

            lakeWaterTemp = await _lakeWaterTempRepository.Create(lakeWaterTemp);
            
            return Ok(lakeWaterTemp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing lake temperature data");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

}