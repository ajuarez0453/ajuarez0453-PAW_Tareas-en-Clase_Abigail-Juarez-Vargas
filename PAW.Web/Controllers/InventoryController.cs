using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class InventoryController : Controller
{
	private readonly IInventoryService _inventoryService;


	public InventoryController(
		IInventoryService inventoryService)
	{
		_inventoryService = inventoryService;
	}


	// INDEX
	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var inventories =
			(await _inventoryService.GetInventoriesAsync())
			.ToList();

		int totalItems = inventories.Count;

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

		var inventoriesForPage =
			inventories
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(inventoriesForPage);
	}


	// CREATE - GET
	[HttpGet]
	public IActionResult Create()
	{
		return View(new InventoryDTO());
	}


	// CREATE - POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		InventoryDTO inventory)
	{
		if (!ModelState.IsValid)
		{
			return View(inventory);
		}

		await _inventoryService.CreateInventoryAsync(inventory);

		return RedirectToAction(nameof(Index));
	}


	// EDIT - GET
	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var inventory =
			await _inventoryService.GetInventoryAsync(id);

		if (inventory == null)
		{
			return NotFound();
		}

		return View(inventory);
	}


	// EDIT - POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		InventoryDTO inventory)
	{
		if (!ModelState.IsValid)
		{
			return View(inventory);
		}

		await _inventoryService.UpdateInventoryAsync(inventory);

		return RedirectToAction(nameof(Index));
	}


	// DETAILS - PARTIAL VIEW
	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var inventory =
			await _inventoryService.GetInventoryAsync(id);

		if (inventory == null)
		{
			return NotFound();
		}

		return PartialView("_Details", inventory);
	}


	// DELETE
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _inventoryService.DeleteInventoryAsync(id);

		return RedirectToAction(nameof(Index));
	}
}