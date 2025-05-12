using dotnet_api.Data.Entities;
using dotnet_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_api.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class DataController(IDataService dataService) : ControllerBase
{
    private readonly IDataService _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(_dataService.Get());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok(_dataService.Get(id));
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] DataObj data)
    {
        var returned = _dataService.Create(data);
        return CreatedAtAction(nameof(Get), new { id = returned.Id }, returned);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id, [FromBody] DataObj data)
    {
        return Ok(_dataService.Delete(id));
    }
}