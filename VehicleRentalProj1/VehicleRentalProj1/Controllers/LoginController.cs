using VehicleRentalProj1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalProj1.Models;

namespace VehicleRentalProj1.Controllers
{
    public class LoginController : Controller
    {

        private readonly VehiclesRent1Context context;

        public LoginController(VehiclesRent1Context context)
        {
            this.context = context;
        }



        public ActionResult Login(Person model)
        {
            Person user = context.Persons.FirstOrDefault(u => u.email == model.email && u.password == model.password);

            if (user != null)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserId", user.PersonId.ToString());

                HttpContext.Session.SetString("UserEmail", user.email);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials";
                return View();
            }
        }

        // GET: LoginController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index", "Home"); // Rediriger vers la page d'accueil après la déconnexion
        }



        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}