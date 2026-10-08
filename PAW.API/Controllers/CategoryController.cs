using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
	private readonly ICategoryRepository _categoryRepository;

	public CategoryController(
		ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}


	// GET: /Category
	[HttpGet]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
	{
		var categories =
			await _categoryRepository.ReadAsync();

		var result =
			categories.Select(CategoryDTO.ConvertFrom);

		return Ok(result);
	}


	// GET: /Category/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<CategoryDTO>> GetById(int id)
	{
		var category =
			await _categoryRepository.FindAsync(id);

		if (category == null)
		{
			return NotFound();
		}

		return Ok(CategoryDTO.ConvertFrom(category));
	}


	// POST: /Category
	[HttpPost]
	public async Task<IActionResult> Create(CategoryDTO dto)
	{
		var category =
			CategoryDTO.ConvertTo(dto);

		category.LastModified = DateTime.Now;

		var result =
			await _categoryRepository.CreateAsync(category);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// PUT: /Category/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		CategoryDTO dto)
	{
		var category =
			await _categoryRepository.FindAsync(id);

		if (category == null)
		{
			return NotFound();
		}

		category.CategoryName =
			dto.CategoryName;

		category.Description =
			dto.Description;

		category.ModifiedBy =
			dto.ModifiedBy;

		category.LastModified =
			DateTime.Now;

		var result =
			await _categoryRepository.UpdateAsync(category);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// DELETE: /Category/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var category =
			await _categoryRepository.FindAsync(id);

		if (category == null)
		{
			return NotFound();
		}

		var result =
			await _categoryRepository.DeleteAsync(category);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}