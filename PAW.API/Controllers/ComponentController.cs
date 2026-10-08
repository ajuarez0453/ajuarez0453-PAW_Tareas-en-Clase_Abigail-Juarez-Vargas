using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
	private readonly IComponentRepository _componentRepository;


	public ComponentController(
		IComponentRepository componentRepository)
	{
		_componentRepository = componentRepository;
	}


	// GET: /Component
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetAll()
	{
		var components =
			await _componentRepository.ReadAsync();

		return Ok(
			components.Select(ComponentDTO.ConvertFrom));
	}


	// GET: /Component/5
	[HttpGet("{id}")]
	public async Task<ActionResult<ComponentDTO>> GetById(decimal id)
	{
		var components =
			await _componentRepository.ReadAsync();

		var component =
			components.FirstOrDefault(x => x.Id == id);

		if (component == null)
		{
			return NotFound();
		}

		return Ok(ComponentDTO.ConvertFrom(component));
	}


	// POST: /Component
	[HttpPost]
	public async Task<IActionResult> Create(ComponentDTO dto)
	{
		var component =
			ComponentDTO.ConvertTo(dto);

		var result =
			await _componentRepository.CreateAsync(component);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// PUT: /Component/5
	[HttpPut("{id}")]
	public async Task<IActionResult> Edit(
		decimal id,
		ComponentDTO dto)
	{
		var components =
			await _componentRepository.ReadAsync();

		var component =
			components.FirstOrDefault(x => x.Id == id);

		if (component == null)
		{
			return NotFound();
		}

		component.Name = dto.Name ?? string.Empty;
		component.Content = dto.Content ?? string.Empty;

		var result =
			await _componentRepository.UpdateAsync(component);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// DELETE: /Component/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(decimal id)
	{
		var components =
			await _componentRepository.ReadAsync();

		var component =
			components.FirstOrDefault(x => x.Id == id);

		if (component == null)
		{
			return NotFound();
		}

		var result =
			await _componentRepository.DeleteAsync(component);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}