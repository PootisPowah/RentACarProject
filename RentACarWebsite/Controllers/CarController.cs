using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACarWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using NuGet.Protocol.Core.Types;
using RentACarProject.Models;


namespace MvcProject.Controllers
{
    public class CarController : Controller
    {
        private readonly HttpClient _client;

        public CarController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();

            _client.BaseAddress = new Uri("https://localhost:44354/api/Car/");
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarModel>>(content);

            return View(cars);
        }

        [HttpPost]
        public async Task<IActionResult> GetCarById(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var car = JsonConvert.DeserializeObject<CarModel>(content);
                var carList = new List<CarModel> { car };
                return View("Index", carList);
            }
            else
            {
                ModelState.AddModelError("", "Car not found or API issue");
                return View("Index", new List<CarModel>());
            }

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarModel car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonCar = JsonConvert.SerializeObject(car);
            var content = new StringContent(jsonCar, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "An error occurred while creating the car.");
            return View(car);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<CarModel>();
                return View(content);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarModel updatedCar)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var jsonCar = JsonConvert.SerializeObject(updatedCar);
            var content = new StringContent(jsonCar, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{updatedCar.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["EditMessage"] = $"Car with ID {updatedCar.Id} has been edited successfully";
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await _client.DeleteAsync($"{id}");
                var responseGet = await _client.GetAsync("https://localhost:44354/api/Rent");
                var inRent = await responseGet.Content.ReadAsAsync<IEnumerable<Rent>>();

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteMessage"] = $"Car with ID {id} has been deleted successfully";
                    return RedirectToAction("Index");
                }
                else if (inRent.Any(r=>r.CarId==id))
                {
                    TempData["DeleteMessageError"]= $"Car with ID {id} is in Rents, delete the car in Rents.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete car");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
    }
    
}
 

