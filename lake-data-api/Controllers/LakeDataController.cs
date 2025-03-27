using Microsoft.AspNetCore.Mvc;

namespace lake_data_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LakeDataController : ControllerBase
{

    private readonly ILogger<LakeDataController> _logger;

    public LakeDataController(ILogger<LakeDataController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok("Hello");
    }
}
