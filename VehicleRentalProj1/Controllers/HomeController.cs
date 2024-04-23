using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VehicleRentalProj1.Models;

namespace VehicleRentalProj1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {

            string userName = HttpContext.Session.GetString("FirstName");
            string userSurname = HttpContext.Session.GetString("LastName");

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userSurname))
            {
                ViewBag.UserName = userName;
                ViewBag.UserSurname = userSurname;

            }

            return View();
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