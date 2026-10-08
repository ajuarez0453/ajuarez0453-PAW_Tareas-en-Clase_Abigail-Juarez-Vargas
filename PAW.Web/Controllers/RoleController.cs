using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class RoleController : Controller
{
	private readonly IRoleService _roleService;


	public RoleController(IRoleService roleService)
	{
		_roleService = roleService;
	}


	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var roles =
			(await _roleService.GetRolesAsync())
			.ToList();

		int totalItems = roles.Count;

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

		var rolesForPage =
			roles
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(rolesForPage);
	}


	[HttpGet]
	public IActionResult Create()
	{
		return View(new RoleDTO());
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(RoleDTO role)
	{
		if (!ModelState.IsValid)
		{
			return View(role);
		}

		await _roleService.CreateRoleAsync(role);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var role =
			await _roleService.GetRoleAsync(id);

		if (role == null)
		{
			return NotFound();
		}

		return View(role);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(RoleDTO role)
	{
		if (!ModelState.IsValid)
		{
			return View(role);
		}

		await _roleService.UpdateRoleAsync(role);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var role =
			await _roleService.GetRoleAsync(id);

		if (role == null)
		{
			return NotFound();
		}

		return PartialView("_Details", role);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _roleService.DeleteRoleAsync(id);

		return RedirectToAction(nameof(Index));
	}
}