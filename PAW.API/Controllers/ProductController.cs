using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
	private readonly IProductRepository _productRepository;

	public ProductController(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}


	// GET: /Product
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
	{
		var products = await _productRepository.ReadAsync();

		var result = products.Select(ProductDTO.ConvertFrom);

		return Ok(result);
	}


	// GET: /Product/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<ProductDTO>> GetById(int id)
	{
		var product = await _productRepository.FindAsync(id);

		if (product == null)
		{
			return NotFound();
		}

		return Ok(ProductDTO.ConvertFrom(product));
	}


	// POST: /Product
	[HttpPost]
	public async Task<IActionResult> Create(ProductDTO dto)
	{
		var product = ProductDTO.ConvertTo(dto);

		product.LastModified = DateTime.Now;

		var result = await _productRepository.CreateAsync(product);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// PUT: /Product/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		ProductDTO dto)
	{
		var product = await _productRepository.FindAsync(id);

		if (product == null)
		{
			return NotFound();
		}

		product.ProductName = dto.Name;
		product.Description = dto.Description;
		product.Rating = dto.Rating;
		product.InventoryId = dto.InventoryId;
		product.SupplierId = dto.SupplierId;
		product.CategoryId = dto.CategoryId;
		product.ModifiedBy = dto.ModifiedBy;

		product.LastModified = DateTime.Now;

		var result =
			await _productRepository.UpdateAsync(product);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// DELETE: /Product/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var product = await _productRepository.FindAsync(id);

		if (product == null)
		{
			return NotFound();
		}

		var result =
			await _productRepository.DeleteAsync(product);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}