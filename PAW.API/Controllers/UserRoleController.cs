using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserRoleController : ControllerBase
{
	private readonly IUserRoleRepository _userRoleRepository;

	public UserRoleController(
		IUserRoleRepository userRoleRepository)
	{
		_userRoleRepository = userRoleRepository;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserRoleDTO>>> GetAll()
	{
		var userRoles =
			await _userRoleRepository.ReadAsync();

		return Ok(
			userRoles.Select(UserRoleDTO.ConvertFrom));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<UserRoleDTO>> GetById(decimal id)
	{
		var userRoles =
			await _userRoleRepository.ReadAsync();

		var userRole =
			userRoles.FirstOrDefault(x => x.Id == id);

		if (userRole == null)
		{
			return NotFound();
		}

		return Ok(UserRoleDTO.ConvertFrom(userRole));
	}
}