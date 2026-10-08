using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly IInventoryService _inventoryService;
		private readonly ISupplierService _supplierService;
		private readonly ILogger<ProductController> _logger;

		public ProductController(
	IProductService productService,
	ICategoryService categoryService,
	IInventoryService inventoryService,
	ISupplierService supplierService,
	ILogger<ProductController> logger)
		{
			_productService = productService;
			_categoryService = categoryService;
			_inventoryService = inventoryService;
			_supplierService = supplierService;
			_logger = logger;
		}

		// =========================================================
		// INDEX
		// =========================================================
		public async Task<IActionResult> Index(int page = 1)
		{
			const int pageSize = 25;

			var products =
				(await _productService.GetProductsAsync())
				.ToList();

			int totalItems = products.Count;

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

			var productsForPage =
				products
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToList();

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(productsForPage);
		}


		// =========================================================
		// CREATE - GET
		// =========================================================
		[HttpGet]
		public IActionResult Create()
		{
			return View(new ProductDTO());
		}


		// =========================================================
		// CREATE - POST
		// =========================================================
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductDTO product)
		{
			if (!ModelState.IsValid)
			{
				return View(product);
			}

			await _productService.CreateProductAsync(product);

			return RedirectToAction(nameof(Index));
		}


		// =========================================================
		// EDIT - GET
		// =========================================================
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var product =
				await _productService.GetProductAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}


		// =========================================================
		// EDIT - POST
		// =========================================================
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductDTO product)
		{
			if (!ModelState.IsValid)
			{
				return View(product);
			}

			await _productService.UpdateProductAsync(product);

			return RedirectToAction(nameof(Index));
		}


		// =========================================================
		// DETAILS - PARTIAL VIEW
		// =========================================================
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var product =
				await _productService.GetProductAsync(id);

			if (product == null)
			{
				return NotFound();
			}


			CategoryDTO? category = null;

			if (product.CategoryId.HasValue)
			{
				category =
					await _categoryService.GetCategoryAsync(
						product.CategoryId.Value);
			}


			InventoryDTO? inventory = null;

			if (product.InventoryId.HasValue)
			{
				inventory =
					await _inventoryService.GetInventoryAsync(
						product.InventoryId.Value);
			}


			SupplierDTO? supplier = null;

			if (product.SupplierId.HasValue)
			{
				supplier =
					await _supplierService.GetSupplierAsync(
						product.SupplierId.Value);
			}


			var details =
				new ProductDetailsDTO
				{
					Product = product,
					Category = category,
					Inventory = inventory,
					Supplier = supplier
				};


			return PartialView("_Details", details);
		}


		// =========================================================
		// DELETE
		// =========================================================
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			await _productService.DeleteProductAsync(id);

			return RedirectToAction(nameof(Index));
		}


		// =========================================================
		// ERROR
		// =========================================================
		[ResponseCache(
			Duration = 0,
			Location = ResponseCacheLocation.None,
			NoStore = true)]
		public IActionResult Error()
		{
			return View(
				new ErrorViewModel
				{
					RequestId =
						Activity.Current?.Id ??
						HttpContext.TraceIdentifier
				});
		}
	}
}