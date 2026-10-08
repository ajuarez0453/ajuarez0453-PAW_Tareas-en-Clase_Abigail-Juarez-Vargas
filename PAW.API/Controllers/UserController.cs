using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	private readonly IUserRepository _userRepository;

	public UserController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
	{
		var users =
			await _userRepository.ReadAsync();

		return Ok(
			users.Select(UserDTO.ConvertFrom));
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<UserDTO>> GetById(int id)
	{
		var user =
			await _userRepository.FindAsync(id);

		if (user == null)
		{
			return NotFound();
		}

		return Ok(UserDTO.ConvertFrom(user));
	}

	[HttpPost]
	public async Task<IActionResult> Create(UserDTO dto)
	{
		var user =
			UserDTO.ConvertTo(dto);

		user.CreatedAt ??= DateTime.Now;

		var result =
			await _userRepository.CreateAsync(user);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		UserDTO dto)
	{
		var user =
			await _userRepository.FindAsync(id);

		if (user == null)
		{
			return NotFound();
		}

		user.Username = dto.Username;
		user.Email = dto.Email;
		user.PasswordHash = dto.PasswordHash;
		user.IsActive = dto.IsActive;
		user.RoleId = dto.RoleId;
		user.LastModifiedBy = dto.LastModifiedBy;


		var result =
			await _userRepository.UpdateAsync(user);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var user =
			await _userRepository.FindAsync(id);

		if (user == null)
		{
			return NotFound();
		}

		var result =
			await _userRepository.DeleteAsync(user);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}