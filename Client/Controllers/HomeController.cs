using Client.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _tokenService = tokenService;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            var postList = new List<Post>();
            return View(postList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> TestWeather()
        {
            var weatherList = new List<TestWeather>();
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var client = new HttpClient(handler))
            {
                var tokenResponse = await _tokenService.GetToken("weatherapi.read");
                client.SetBearerToken(tokenResponse.AccessToken);
                var result = client.GetAsync("https://localhost:7080/weather").Result;

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