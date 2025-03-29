using Microsoft.AspNetCore.Mvc;
using lake_data_api.Services;
using lake_data_api.Models;

namespace lake_data_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LakeController : ControllerBase
{
    private readonly ILogger<LakeController> _logger;
    private readonly ILakeWaterLevelService _lakeWaterLevelService;
    private readonly ILakeRepository _lakeRepository;   

    public LakeController(ILogger<LakeController> logger, 
    ILakeWaterLevelService lakeWaterLevelService,
    ILakeRepository lakeRepository)
    {
        _logger = logger;
        _lakeWaterLevelService = lakeWaterLevelService;
        _lakeRepository = lakeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            var lakes = await _lakeRepository.List();
            if (lakes == null || !lakes.Any())
            {
                return NotFound("No lakes found");
            }
            
            return Ok(lakes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lake list");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById(string id)
    {
        try
        {
            var lake = await _lakeRepository.FindById(id);
            if (lake == null)
            {
                return NotFound("Lake not found");
            }
            
            return Ok(lake);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding lake for {id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Lake lake)
    {
        try
        {
            // Don't trust passed in guid, generate a new one
            lake.Id = Guid.NewGuid().ToString();
            var createdLake = await _lakeRepository.Create(lake);
            if (createdLake == null)
            {
                return BadRequest("Failed to create lake");
            }
            
            return Ok(createdLake);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating lake for {id}", lake.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Lake lake)
    {
        try
        {
            if (id != lake.Id)
            {
                return BadRequest("Lake ID mismatch");
            }
            
            var existingLake = await _lakeRepository.FindById(id);
            if (existingLake == null)
            {
                return NotFound("Lake not found");
            }

            var returnLake = await _lakeRepository.Update(lake);

            return Ok(returnLake);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating lake for {id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _lakeRepository.Delete(id);
            return Ok("Lake deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting lake for {id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
