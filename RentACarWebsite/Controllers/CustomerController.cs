using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACarProject.Models;
using RentACarWebsite.Models;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;

namespace RentACarWebsite.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _client;
        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44354/api/Customer/");
        }
        // GET: CustomerController
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<CustomerModel>>(content);
            return View(customers);
        }
        [HttpPost]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<CustomerModel>(content);
                var customerList = new List<CustomerModel> {customer};
                return View("Index", customerList);
            }
            else
            {
                ModelState.AddModelError("", "Customer not found or API error");
                return View("Index",new List<CustomerModel>());
            }

           
        }
       

        // GET: CustomerController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CustomerModel customer)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var jsonCustomer = JsonConvert.SerializeObject(customer);
            var content = new StringContent(jsonCustomer, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "An error occurred while creating the customer.");
            return View(customer);
        }

        // GET: CustomerController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<CustomerModel>();
                return View(content);

            }
            return View("Error");

        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerModel updatedCustomer)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var jsonCustomer = JsonConvert.SerializeObject(updatedCustomer);
            var content = new StringContent(jsonCustomer,Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{updatedCustomer.Id}",content);
            if (response.IsSuccessStatusCode)
            {
                TempData["EditMessage"] = $"Customer with {updatedCustomer.Id} has been edited successfully";
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
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
                    TempData["DeleteMessage"] = $"Customer with ID: {id} has been successfully deleted";
                    return RedirectToAction("Index");
                }
                else if (inRent.Any(r=>r.CustomerId == id))
                {
                    TempData["DeleteMessageError"] = $"Customer with ID: {id} is in Rents, delete the Rent before deleting the customer.";
                    return RedirectToAction("Index");
                }
                else 
                {
                    ModelState.AddModelError("", "Failed to delete customer");
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
