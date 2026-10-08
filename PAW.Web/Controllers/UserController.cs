using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class UserController : Controller
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var users =
			(await _userService.GetUsersAsync())
			.ToList();

		int totalItems = users.Count;

		int totalPages =
			(int)Math.Ceiling(
				totalItems / (double)pageSize);

		if (page < 1)
		{
			page = 1;
		}

		if (totalPages > 0 && page > totalPages)
		{
			page = totalPages;
		}

		var usersForPage =
			users
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(usersForPage);
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View(new UserDTO());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(UserDTO user)
	{
		if (!ModelState.IsValid)
		{
			return View(user);
		}

		await _userService.CreateUserAsync(user);

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var user =
			await _userService.GetUserAsync(id);

		if (user == null)
		{
			return NotFound();
		}

		return View(user);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(UserDTO user)
	{
		if (!ModelState.IsValid)
		{
			return View(user);
		}

		await _userService.UpdateUserAsync(user);

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var user =
			await _userService.GetUserAsync(id);

		if (user == null)
		{
			return NotFound();
		}

		return PartialView("_Details", user);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _userService.DeleteUserAsync(id);

		return RedirectToAction(nameof(Index));
	}
}