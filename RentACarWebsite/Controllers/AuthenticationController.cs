using Microsoft.AspNetCore.Mvc;

namespace RentACarWebsite.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
