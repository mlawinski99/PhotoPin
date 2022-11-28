﻿using Client.Models;
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
    [Authorize]
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

				var lists = new ListsViewModel
				{
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
		public async Task<IActionResult> Favourites()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/favourite");

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

            request.Content = JsonContent.Create(new { id = id});

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index", "Home");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            throw new Exception("Can't connect to API");

            //var postList = new List<Post>();
            //return View(postList);
        }

		public async Task<IActionResult> Delete(int id)
		{
			var httpClient = _httpClientFactory.CreateClient("APIClient");

			var request = new HttpRequestMessage(
				HttpMethod.Delete,
				$"/api/posts/{id}");

			request.Content = JsonContent.Create(new { id = id });

			var response = await httpClient.SendAsync(
				request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

			if(response.EnsureSuccessStatusCode().IsSuccessStatusCode)
				return RedirectToAction("MyProfile", "User");


			throw new Exception("Can't connect to API");
		}
	}
}
