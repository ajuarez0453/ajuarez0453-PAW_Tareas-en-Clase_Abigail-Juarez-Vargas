using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class CategoryController : Controller
{
	private readonly ICategoryService _categoryService;


	public CategoryController(
		ICategoryService categoryService)
	{
		_categoryService = categoryService;
	}


	// =========================================================
	// INDEX
	// =========================================================

	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var categories =
			(await _categoryService.GetCategoriesAsync())
			.ToList();

		int totalItems = categories.Count;

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

		var categoriesForPage =
			categories
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(categoriesForPage);
	}


	// =========================================================
	// CREATE - GET
	// =========================================================

	[HttpGet]
	public IActionResult Create()
	{
		return View(new CategoryDTO());
	}


	// =========================================================
	// CREATE - POST
	// =========================================================

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		CategoryDTO category)
	{
		if (!ModelState.IsValid)
		{
			return View(category);
		}

		await _categoryService.CreateCategoryAsync(category);

		return RedirectToAction(nameof(Index));
	}


	// =========================================================
	// EDIT - GET
	// =========================================================

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var category =
			await _categoryService.GetCategoryAsync(id);

		if (category == null)
		{
			return NotFound();
		}

		return View(category);
	}


	// =========================================================
	// EDIT - POST
	// =========================================================

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		CategoryDTO category)
	{
		if (!ModelState.IsValid)
		{
			return View(category);
		}

		await _categoryService.UpdateCategoryAsync(category);

		return RedirectToAction(nameof(Index));
	}


	// =========================================================
	// DETAILS - PARTIAL VIEW
	// =========================================================

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var category =
			await _categoryService.GetCategoryAsync(id);

		if (category == null)
		{
			return NotFound();
		}

		return PartialView("_Details", category);
	}


	// =========================================================
	// DELETE
	// =========================================================

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _categoryService.DeleteCategoryAsync(id);

		return RedirectToAction(nameof(Index));
	}
}