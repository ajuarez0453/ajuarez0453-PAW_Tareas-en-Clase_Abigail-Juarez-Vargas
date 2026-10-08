using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
	private readonly IInventoryRepository _inventoryRepository;

	public InventoryController(
		IInventoryRepository inventoryRepository)
	{
		_inventoryRepository = inventoryRepository;
	}


	// GET: /Inventory
	[HttpGet]
	public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetAll()
	{
		var inventories =
			await _inventoryRepository.ReadAsync();

		var result =
			inventories.Select(InventoryDTO.ConvertFrom);

		return Ok(result);
	}


	// GET: /Inventory/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<InventoryDTO>> GetById(int id)
	{
		var inventory =
			await _inventoryRepository.FindAsync(id);

		if (inventory == null)
		{
			return NotFound();
		}

		return Ok(InventoryDTO.ConvertFrom(inventory));
	}


	// POST: /Inventory
	[HttpPost]
	public async Task<IActionResult> Create(InventoryDTO dto)
	{
		var inventory =
			InventoryDTO.ConvertTo(dto);

		inventory.DateAdded ??= DateTime.Now;
		inventory.LastUpdated = DateTime.Now;

		var result =
			await _inventoryRepository.CreateAsync(inventory);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// PUT: /Inventory/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		InventoryDTO dto)
	{
		var inventory =
			await _inventoryRepository.FindAsync(id);

		if (inventory == null)
		{
			return NotFound();
		}

		inventory.UnitPrice = dto.UnitPrice;
		inventory.UnitsInStock = dto.UnitsInStock;
		inventory.ProductId = dto.ProductId;
		inventory.ModifiedBy = dto.ModifiedBy;
		inventory.LastUpdated = DateTime.Now;

		var result =
			await _inventoryRepository.UpdateAsync(inventory);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// DELETE: /Inventory/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var inventory =
			await _inventoryRepository.FindAsync(id);

		if (inventory == null)
		{
			return NotFound();
		}

		var result =
			await _inventoryRepository.DeleteAsync(inventory);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}