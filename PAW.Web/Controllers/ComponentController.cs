using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class ComponentController : Controller
{
	private readonly IComponentService _componentService;


	public ComponentController(
		IComponentService componentService)
	{
		_componentService = componentService;
	}


	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var components =
			(await _componentService.GetComponentsAsync())
			.ToList();

		int totalItems = components.Count;

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

		var componentsForPage =
			components
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(componentsForPage);
	}


	[HttpGet]
	public IActionResult Create()
	{
		return View(new ComponentDTO());
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		ComponentDTO component)
	{
		if (!ModelState.IsValid)
		{
			return View(component);
		}

		await _componentService.CreateComponentAsync(component);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Edit(decimal id)
	{
		var component =
			await _componentService.GetComponentAsync(id);

		if (component == null)
		{
			return NotFound();
		}

		return View(component);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		ComponentDTO component)
	{
		if (!ModelState.IsValid)
		{
			return View(component);
		}

		await _componentService.UpdateComponentAsync(component);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Details(decimal id)
	{
		var component =
			await _componentService.GetComponentAsync(id);

		if (component == null)
		{
			return NotFound();
		}

		return PartialView("_Details", component);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(decimal id)
	{
		await _componentService.DeleteComponentAsync(id);

		return RedirectToAction(nameof(Index));
	}
}
