using Client.Models;
using Client.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client.Controllers
{
   // [Authorize]
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
        }
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/posts/{id}");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsStringAsync();
                return View(JsonConvert.DeserializeObject<Post>(model));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            throw new Exception("Can't connect to API");

            //var postList = new List<Post>();
            //return View(postList);
        }

        public async Task<IActionResult> LikeUnlike(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"/api/favourite");

            //string data = "{\"id\":\""+id+"\"}";
            request.Content = JsonContent.Create(new { id = id});

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return View();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            throw new Exception("Can't connect to API");

            //var postList = new List<Post>();
            //return View(postList);
        }
    }
}
