using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.City;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CityController(ICityService cityService) : ControllerBase
{
    private readonly ICityService _cityService = cityService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
    {
        var cities = await _cityService.GetAllAsync();
        return Ok(cities);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> GetById(int id)
    {
        var city = await _cityService.GetByIdAsync(id);
        if (city is null)
        {
            return NotFound(new { message = $"City with ID {id} not found" });
        }

        return Ok(city);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> GetByName(string name)
    {
        var city = await _cityService.GetByNameAsync(name);
        if (city is null)
        {
            return NotFound(new { message = $"City with name '{name}' not found" });
        }

        return Ok(city);
    }

    [HttpGet("country/{country}")]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetByCountry(string country)
    {
        var cities = await _cityService.GetByCountryAsync(country);
        return Ok(cities);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CityDto>> Create([FromBody] CreateCityDto createDto)
    {
        try
        {
            var city = await _cityService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = city.CityId }, city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> Update(int id, [FromBody] UpdateCityDto updateDto)
    {
        try
        {
            var city = await _cityService.UpdateAsync(id, updateDto);
            return Ok(city);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _cityService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}