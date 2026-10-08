using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class UserActionController : Controller
{
	private readonly IUserActionService _userActionService;

	public UserActionController(
		IUserActionService userActionService)
	{
		_userActionService = userActionService;
	}

	public async Task<IActionResult> Index()
	{
		var actions =
			await _userActionService.GetUserActionsAsync();

		return View(actions);
	}

	[HttpGet]
	public async Task<IActionResult> Details(decimal id)
	{
		var action =
			await _userActionService.GetUserActionAsync(id);

		if (action == null)
			return NotFound();

		return PartialView("_Details", action);
	}
}