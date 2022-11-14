using Client.Models;
using Client.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client.Controllers
{
	public class PostController : Controller
	{
        private readonly IHttpClientFactory _httpClientFactory;

        public PostController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");


            byte[] data;
            using (var br = new BinaryReader(model.Image.OpenReadStream()))
            {
                data = br.ReadBytes((int)model.Image.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, "Image", model.Image.FileName);
            multiContent.Add(new StringContent(model.Description), "Description" +
                "");


            var response = await httpClient.PostAsync("/api/posts", multiContent);
            Console.WriteLine(response.StatusCode);
            return RedirectToAction("Index", "Home");


            /*
            using (var client = new HttpClient())
            {
             //   var tokenResponse = await _tokenService.GetToken("weatherapi.read");
               // client.SetBearerToken(tokenResponse.AccessToken);
                var result = await client.PostAsJsonAsync("https://localhost:7080/posts", model);
                if (result.IsSuccessStatusCode)
                    return View(model);

                throw new Exception("Can't connect to API");
            }
            return View();*/
        }
    }
}
