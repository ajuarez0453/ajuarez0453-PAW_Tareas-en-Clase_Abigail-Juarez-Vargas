using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController : ControllerBase
{
	private readonly ISupplierRepository _supplierRepository;

	public SupplierController(
		ISupplierRepository supplierRepository)
	{
		_supplierRepository = supplierRepository;
	}


	// GET: /Supplier
	[HttpGet]
	public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAll()
	{
		var suppliers =
			await _supplierRepository.ReadAsync();

		var result =
			suppliers.Select(SupplierDTO.ConvertFrom);

		return Ok(result);
	}


	// GET: /Supplier/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<SupplierDTO>> GetById(int id)
	{
		var supplier =
			await _supplierRepository.FindAsync(id);

		if (supplier == null)
		{
			return NotFound();
		}

		return Ok(SupplierDTO.ConvertFrom(supplier));
	}


	// POST: /Supplier
	[HttpPost]
	public async Task<IActionResult> Create(SupplierDTO dto)
	{
		var supplier =
			SupplierDTO.ConvertTo(dto);

		supplier.LastModified = DateTime.Now;

		var result =
			await _supplierRepository.CreateAsync(supplier);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// PUT: /Supplier/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		SupplierDTO dto)
	{
		var supplier =
			await _supplierRepository.FindAsync(id);

		if (supplier == null)
		{
			return NotFound();
		}

		supplier.SupplierName = dto.SupplierName;
		supplier.ContactName = dto.ContactName;
		supplier.ContactTitle = dto.ContactTitle;
		supplier.Phone = dto.Phone;
		supplier.Address = dto.Address;
		supplier.City = dto.City;
		supplier.Country = dto.Country;
		supplier.ModifiedBy = dto.ModifiedBy;
		supplier.LastModified = DateTime.Now;

		var result =
			await _supplierRepository.UpdateAsync(supplier);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	// DELETE: /Supplier/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var supplier =
			await _supplierRepository.FindAsync(id);

		if (supplier == null)
		{
			return NotFound();
		}

		var result =
			await _supplierRepository.DeleteAsync(supplier);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}