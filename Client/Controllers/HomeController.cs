using Client.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> TestWeather()
        {
            var weatherList = new List<TestWeather>();
            using(var client = new HttpClient())
            {
                var tokenResponse = await _tokenService.GetToken("weatherapi.read");
                client.SetBearerToken(tokenResponse.AccessToken);

                var result = client.GetAsync("https://localhost:7061/WeatherForecast").Result;

                if(result.IsSuccessStatusCode)
                {
                    var model = result.Content.ReadAsStringAsync().Result;
                    weatherList = JsonConvert.DeserializeObject<List<TestWeather>>(model);
                    return View(weatherList);
                }

                throw new Exception("Can't connect to API");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}