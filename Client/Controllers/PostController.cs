using Client.Models;
using Client.Services;
using Client.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
	public class PostController : Controller
	{
        private readonly ITokenService _tokenService;

        public PostController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await _tokenService.GetToken("weatherapi.read");
                client.SetBearerToken(tokenResponse.AccessToken);
                var result = await client.PostAsJsonAsync("https://localhost:7080/posts", model);
                if (result.IsSuccessStatusCode)
                    return View(model);

                throw new Exception("Can't connect to API");
            }
            return View();
        }
    }
}
