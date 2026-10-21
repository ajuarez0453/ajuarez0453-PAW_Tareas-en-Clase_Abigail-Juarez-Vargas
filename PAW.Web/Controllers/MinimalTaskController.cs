using Microsoft.AspNetCore.Mvc;

namespace PAW.Web.Controllers
{
	public class MinimalTasksController : Controller
	{
		// Este controller solamente muestra la pantalla.
		// Los datos se consultan mediante JavaScript.
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}