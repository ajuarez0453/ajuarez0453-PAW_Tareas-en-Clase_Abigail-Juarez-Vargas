using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
	private readonly INotificationRepository _notificationRepository;

	public NotificationController(
		INotificationRepository notificationRepository)
	{
		_notificationRepository = notificationRepository;
	}


	[HttpGet]
	public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetAll()
	{
		var notifications =
			await _notificationRepository.ReadAsync();

		return Ok(
			notifications.Select(NotificationDTO.ConvertFrom));
	}


	[HttpGet("{id:int}")]
	public async Task<ActionResult<NotificationDTO>> GetById(int id)
	{
		var notification =
			await _notificationRepository.FindAsync(id);

		if (notification == null)
		{
			return NotFound();
		}

		return Ok(NotificationDTO.ConvertFrom(notification));
	}


	[HttpPost]
	public async Task<IActionResult> Create(NotificationDTO dto)
	{
		var notification =
			NotificationDTO.ConvertTo(dto);

		notification.CreatedAt ??= DateTime.Now;

		var result =
			await _notificationRepository.CreateAsync(notification);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpPut("{id:int}")]
	public async Task<IActionResult> Edit(
		int id,
		NotificationDTO dto)
	{
		var notification =
			await _notificationRepository.FindAsync(id);

		if (notification == null)
		{
			return NotFound();
		}

		notification.UserId = dto.UserId;
		notification.Message = dto.Message ?? string.Empty;
		notification.IsRead = dto.IsRead;

		var result =
			await _notificationRepository.UpdateAsync(notification);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}


	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var notification =
			await _notificationRepository.FindAsync(id);

		if (notification == null)
		{
			return NotFound();
		}

		var result =
			await _notificationRepository.DeleteAsync(notification);

		if (!result)
		{
			return BadRequest();
		}

		return Ok();
	}
}