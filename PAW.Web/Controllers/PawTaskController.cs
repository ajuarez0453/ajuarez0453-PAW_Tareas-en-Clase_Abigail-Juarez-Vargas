using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class PawTaskController : Controller
{
	private readonly IPawTaskService _taskService;


	public PawTaskController(
		IPawTaskService taskService)
	{
		_taskService = taskService;
	}


	public async Task<IActionResult> Index(int page = 1)
	{
		const int pageSize = 25;

		var tasks =
			(await _taskService.GetTasksAsync())
			.ToList();

		int totalItems = tasks.Count;

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

		var tasksForPage =
			tasks
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(tasksForPage);
	}


	[HttpGet]
	public IActionResult Create()
	{
		return View(new PawTaskDTO());
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		PawTaskDTO task)
	{
		if (!ModelState.IsValid)
		{
			return View(task);
		}

		await _taskService.CreateTaskAsync(task);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var task =
			await _taskService.GetTaskAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		return View(task);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		PawTaskDTO task)
	{
		if (!ModelState.IsValid)
		{
			return View(task);
		}

		await _taskService.UpdateTaskAsync(task);

		return RedirectToAction(nameof(Index));
	}


	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var task =
			await _taskService.GetTaskAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		return PartialView("_Details", task);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		await _taskService.DeleteTaskAsync(id);

		return RedirectToAction(nameof(Index));
	}
}