using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PawTaskController : ControllerBase
{
	private readonly IPawTaskRepository _taskRepository;

	public PawTaskController(
		IPawTaskRepository taskRepository)
	{
		_taskRepository = taskRepository;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<PawTaskDTO>>> GetAll()
	{
		var tasks =
			await _taskRepository.ReadAsync();

		return Ok(
			tasks.Select(PawTaskDTO.ConvertFrom));
	}


	[HttpGet("{id:int}")]
	public async Task<ActionResult<PawTaskDTO>> GetById(int id)
	{
		var task =
			await _taskRepository.FindAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		return Ok(PawTaskDTO.ConvertFrom(task));
	}


	[HttpPost]
	public async Task<IActionResult> Create(PawTaskDTO dto)
	{
		var task =
			PawTaskDTO.ConvertTo(dto);

		task.CreatedAt ??= DateTime.Now;
		task.LastModified = DateTime.Now;

		var result =
			await _taskRepository.CreateAsync(task);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		PawTaskDTO dto)
	{
		var task =
			await _taskRepository.FindAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		task.Name = dto.Name;
		task.Description = dto.Description;
		task.Status = dto.Status;
		task.DueDate = dto.DueDate;
		task.ModifiedBy = dto.ModifiedBy;
		task.LastModified = DateTime.Now;

		var result =
			await _taskRepository.UpdateAsync(task);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var task =
			await _taskRepository.FindAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		var result =
			await _taskRepository.DeleteAsync(task);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}