using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Client.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> MyProfile()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/posts/user");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsStringAsync();
                return View(JsonConvert.DeserializeObject<List<Post>>(model));
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
    }
}
