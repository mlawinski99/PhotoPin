using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;
namespace Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var requestPosts = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/posts/all");

            var responsePosts = await httpClient.SendAsync(
                requestPosts, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var requestFavourites = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/favourite");

			var responseFavourites = await httpClient.SendAsync(
                requestFavourites, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            
            if (responsePosts.IsSuccessStatusCode && responseFavourites.IsSuccessStatusCode)
            {
                var modelPost = await responsePosts.Content.ReadAsStringAsync();
                var modelFavourites = await responseFavourites.Content.ReadAsStringAsync();

                var lists = new ListsViewModel {
                    Favourites = JsonConvert.DeserializeObject<List<Post>>(modelFavourites),
                    Posts = JsonConvert.DeserializeObject<List<Post>>(modelPost)
                };
                //var likeList = new List<int>(); 
				foreach (var post in lists.Posts)
				{
					var requestLikeCount = new HttpRequestMessage(
						HttpMethod.Get,
						$"api/favourite/{post.Id}");
					var responseLikeCount = await httpClient.SendAsync(
						requestLikeCount, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
					var modelLikes = await responseLikeCount.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<LikeCountViewModel>(modelLikes);

					post.likeCount = model.count;
				}
                //lists.LikeCount = likeList;
				return View(lists);
            }
            else if (responsePosts.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    responsePosts.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                   responseFavourites.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                   responseFavourites.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            throw new Exception("Can't connect to API");

            //var postList = new List<Post>();
            //return View(postList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}