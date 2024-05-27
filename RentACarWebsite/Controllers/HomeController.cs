using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACarWebsite.Models;
using System.Diagnostics;
using System.Text;

namespace RentACarWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44354/api/");
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoginUser(UserInfo user)
        {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8,"application/json");
            var response = await _client.PostAsync("Token", stringContent);
                
                    string token = await response.Content.ReadAsStringAsync();
                    if (token =="Invalid credentials")
                    {
                        ModelState.AddModelError("Username","Incorrect Username or Password");
                        return View("Index");
            }
                    HttpContext.Session.SetString("JWToken", token);
                
                return Redirect("~/Authentication/Index");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserInfo userInfo)
        {
            var response = await _client.GetAsync("https://localhost:44354/api/UserInfoes");
            var existingUsers = await response.Content.ReadAsAsync<IEnumerable<UserInfo>>();

            // Check if the username already exists
            if (existingUsers.Any(u => u.Username == userInfo.Username))
            {
                // Username already exists, return error message and stay on registration page
                ModelState.AddModelError("Username", "Username already exists.");
                return View("Register");
            }
            else
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");
                var responsePost = await _client.PostAsync("https://localhost:44354/api/UserInfoes", stringContent);
                string token = await responsePost.Content.ReadAsStringAsync();
                return Redirect("~/Home/Index");
            }
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();//clear token
            return Redirect("~/Home/Index");
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
