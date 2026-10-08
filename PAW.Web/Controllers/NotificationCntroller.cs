using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class NotificationController : Controller
{
	private readonly INotificationService _notificationService;


	public NotificationController(
		INotificationService notificationService)
	{
		_notificationService = notificationService;
	}


	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var notifications =
			(await _notificationService.GetNotificationsAsync())
			.ToList();

		int totalItems = notifications.Count;

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

		var notificationsForPage =
			notifications
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(notificationsForPage);
	}


	[HttpGet]
	public IActionResult Create()
	{
		return View(new NotificationDTO());
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		NotificationDTO notification)
	{
		if (!ModelState.IsValid)
		{
			return View(notification);
		}

		await _notificationService
			.CreateNotificationAsync(notification);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var notification =
			await _notificationService
				.GetNotificationAsync(id);

		if (notification == null)
		{
			return NotFound();
		}

		return View(notification);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		NotificationDTO notification)
	{
		if (!ModelState.IsValid)
		{
			return View(notification);
		}

		await _notificationService
			.UpdateNotificationAsync(notification);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var notification =
			await _notificationService
				.GetNotificationAsync(id);

		if (notification == null)
		{
			return NotFound();
		}

		return PartialView("_Details", notification);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _notificationService
			.DeleteNotificationAsync(id);

		return RedirectToAction(nameof(Index));
	}
}