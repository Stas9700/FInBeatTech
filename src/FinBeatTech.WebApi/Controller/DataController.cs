using FinBeatTech.DTO;
using FinBeatTech.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinBeatTech.WebApi;

[ApiController]
[Route("api/[controller]")]
public class DataController(IDataService dataService): ControllerBase
{
    [HttpPost("save")]
    public IActionResult SaveData([FromBody] List<Dictionary<int, string>> inputData)
    {
        if (inputData == null || inputData.Count == 0)
        {
            return BadRequest("Input data cannot be empty");
        }

        dataService.SaveData(inputData);
        return Ok();
    }

    [HttpGet("get")]
    public async Task<IReadOnlyCollection<DataEntry>> GetData([FromQuery] int? code)
    {
        var data = await dataService.GetData(code);
        return data;
    }
}