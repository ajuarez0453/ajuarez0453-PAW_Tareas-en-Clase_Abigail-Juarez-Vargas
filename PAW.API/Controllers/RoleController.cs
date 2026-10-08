using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
	private readonly IRoleRepository _roleRepository;

	public RoleController(IRoleRepository roleRepository)
	{
		_roleRepository = roleRepository;
	}


	[HttpGet]
	public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAll()
	{
		var roles =
			await _roleRepository.ReadAsync();

		return Ok(
			roles.Select(RoleDTO.ConvertFrom));
	}


	[HttpGet("{id:int}")]
	public async Task<ActionResult<RoleDTO>> GetById(int id)
	{
		var role =
			await _roleRepository.FindAsync(id);

		if (role == null)
		{
			return NotFound();
		}

		return Ok(RoleDTO.ConvertFrom(role));
	}


	[HttpPost]
	public async Task<IActionResult> Create(RoleDTO dto)
	{
		var role =
			RoleDTO.ConvertTo(dto);

		var result =
			await _roleRepository.CreateAsync(role);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		RoleDTO dto)
	{
		var role =
			await _roleRepository.FindAsync(id);

		if (role == null)
		{
			return NotFound();
		}

		role.RoleName = dto.RoleName ?? string.Empty;

		var result =
			await _roleRepository.UpdateAsync(role);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var role =
			await _roleRepository.FindAsync(id);

		if (role == null)
		{
			return NotFound();
		}

		var result =
			await _roleRepository.DeleteAsync(role);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}