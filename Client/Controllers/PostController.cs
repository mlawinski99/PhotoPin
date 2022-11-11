using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
	public class PostController : Controller
	{
		public IActionResult Create()
		{
			return View();
		}
	}
}
