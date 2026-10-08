using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class SupplierController : Controller
{
	private readonly ISupplierService _supplierService;


	public SupplierController(
		ISupplierService supplierService)
	{
		_supplierService = supplierService;
	}


	// INDEX
	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var suppliers =
			(await _supplierService.GetSuppliersAsync())
			.ToList();

		int totalItems = suppliers.Count;

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

		var suppliersForPage =
			suppliers
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(suppliersForPage);
	}


	// CREATE - GET
	[HttpGet]
	public IActionResult Create()
	{
		return View(new SupplierDTO());
	}


	// CREATE - POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		SupplierDTO supplier)
	{
		if (!ModelState.IsValid)
		{
			return View(supplier);
		}

		await _supplierService.CreateSupplierAsync(supplier);

		return RedirectToAction(nameof(Index));
	}


	// EDIT - GET
	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var supplier =
			await _supplierService.GetSupplierAsync(id);

		if (supplier == null)
		{
			return NotFound();
		}

		return View(supplier);
	}


	// EDIT - POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		SupplierDTO supplier)
	{
		if (!ModelState.IsValid)
		{
			return View(supplier);
		}

		await _supplierService.UpdateSupplierAsync(supplier);

		return RedirectToAction(nameof(Index));
	}


	// DETAILS - PARTIAL VIEW
	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var supplier =
			await _supplierService.GetSupplierAsync(id);

		if (supplier == null)
		{
			return NotFound();
		}

		return PartialView("_Details", supplier);
	}


	// DELETE
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _supplierService.DeleteSupplierAsync(id);

		return RedirectToAction(nameof(Index));
	}
}