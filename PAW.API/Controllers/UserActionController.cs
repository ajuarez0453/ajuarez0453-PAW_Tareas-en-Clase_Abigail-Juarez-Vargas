using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserActionController : ControllerBase
{
	private readonly IUserActionRepository _userActionRepository;

	public UserActionController(
		IUserActionRepository userActionRepository)
	{
		_userActionRepository = userActionRepository;
	}


	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserActionDTO>>> GetAll()
	{
		var actions =
			await _userActionRepository.ReadAsync();

		return Ok(
			actions.Select(UserActionDTO.ConvertFrom));
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<UserActionDTO>> GetById(decimal id)
	{
		var actions =
			await _userActionRepository.ReadAsync();

		var action =
			actions.FirstOrDefault(x => x.Id == id);

		if (action == null)
		{
			return NotFound();
		}

		return Ok(UserActionDTO.ConvertFrom(action));
	}
}