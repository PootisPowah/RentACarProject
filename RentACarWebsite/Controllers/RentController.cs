using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACarProject.Models;
using RentACarWebsite.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace RentACarWebsite.Controllers
{
    public class RentController : Controller
    {
        private readonly HttpClient _client;
        public RentController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44354/api/");
        }

        // GET: RentController1
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var rents = await GetRents();
            var customers = await GetCustomers();
            var cars = await GetCars();

            var viewModel = new RentCustCarModel
            {
                Rent = new RentModel(),  // Single RentModel instance
                Rents = rents,           // Collection of RentModel
                Customers = customers,
                Cars = cars
            };

            return View(viewModel);
        }

        // GET: RentController1/Details/5
        public async Task<IActionResult> GetRentByCusCar(int customerId, int carId)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"Rent/{customerId}/{carId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rent = JsonConvert.DeserializeObject<RentModel>(content);
                var rentList = new List<RentModel> { rent };

                return View("Index", new RentCustCarModel
                {
                    Rent = new RentModel(),  // Single RentModel instance
                    Rents = rentList,
                    Customers = await GetCustomers(),
                    Cars = await GetCars()
                });
            }
            else
            {
                ModelState.AddModelError("", "Rent not found or API issue");
                return View("Index", new RentCustCarModel
                {
                    Rent = new RentModel(),  // Single RentModel instance
                    Rents = new List<RentModel>(),
                    Customers = await GetCustomers(),
                    Cars = await GetCars()
                });
            }
        }

        private async Task<IEnumerable<RentModel>> GetRents()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("Rent");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<RentModel>>(content);
        }
        private async Task<RentModel> GetRent(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"Rent/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RentModel>(content);
        }

        private async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("Customer");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CustomerModel>>(content);
        }

        private async Task<IEnumerable<CarModel>> GetCars()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("Car");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CarModel>>(content);
        }

        // GET: RentController1/Create
        public async Task<IActionResult> Create()
        {

            var viewModel = new RentCustCarModel
            {
                Rent = new RentModel(),
                Customers = await GetCustomers(),
                Cars = await GetCars()
            };
            return View(viewModel);
        }

        // POST: RentController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentCustCarModel viewModel)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (!ModelState.IsValid)
            {
                var jsonRent = JsonConvert.SerializeObject(viewModel.Rent);
                var content = new StringContent(jsonRent, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("Rent", content);
                var responseGet = await _client.GetAsync("https://localhost:44354/api/UserInfoes");
                var availableCar = await responseGet.Content.ReadAsAsync<IEnumerable<Car>>();
                if (response.IsSuccessStatusCode)
                {
                    //TempData["CreateMessage"] = "Rent created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else if (availableCar.Any(c => c.IsAvailable == false))
                    {
                        ModelState.AddModelError("", "Car isn't available for rent.");
                    }
                else
                {
                    ModelState.AddModelError("", "Unable to create rent record. Please try again.");
                }
            }
            else
            {
                ModelState.AddModelError("", "An error occurred when creating the rent");
            }

            // Repopulate the Customers and Cars in case of an error
            viewModel.Customers = await GetCustomers();
            viewModel.Cars = await GetCars();

            return View(viewModel);
        }

        // GET: RentController1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            try
            {
                var rentTask = GetRent(id);
                var customersTask = GetCustomers();
                var carsTask = GetCars();

                await Task.WhenAll(rentTask, customersTask, carsTask);

                var viewModel = new RentCustCarModel
                {
                    Rent = rentTask.Result,
                    Customers = customersTask.Result,
                    Cars = carsTask.Result
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        // POST: RentController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RentCustCarModel updatedRent)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var jsonRent = JsonConvert.SerializeObject(updatedRent.Rent);
           var content = new StringContent(jsonRent, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"Rent/{updatedRent.Rent.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["EditMessage"] = $"Rent {updatedRent.Rent.Id} has been edited successfully";
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(RentModel rentToDelete)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await _client.DeleteAsync($"Rent/{rentToDelete.Id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteMessage"] = $"Rent with ID: {rentToDelete.Id} has been successfully deleted";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete rent");
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
