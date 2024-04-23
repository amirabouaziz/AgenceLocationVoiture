using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleRentalProj1.Models;


namespace VehicleRentalProj1.Controllers
{
    public class RentalController : Controller
    {

        private readonly VehiclesRent1Context context;
        public RentalController(VehiclesRent1Context context)
        {
            this.context = context;
        }
        // GET: RentalController
        public async Task<IActionResult> Index()
        {
            var rentals = await context.Rentals
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .ToListAsync();

            return View(rentals);
        }

        // GET: RentalController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: RentalController/Create
        public IActionResult Create()
        {
            var persons = context.Persons.ToList(); // Get the list of persons (adjust as needed)
            var vehicles = context.Vehicles.ToList(); // Get the list of vehicles (adjust as needed)

            ViewBag.Persons = new SelectList(persons, "PersonId", "FirstName");
            ViewBag.Vehicles = new SelectList(vehicles, "VehicleId", "PlateNumber");

            return View();
        }


        // POST: Rental/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rental rental)
        {
            if (ModelState.IsValid)
            {
                string userIdString = HttpContext.Session.GetString("UserId");

                if (int.TryParse(userIdString, out int loggedInPersonId))
                {
                    rental.PersonId = loggedInPersonId;

                    if (rental.RentalStartDate < DateTime.Today)
                    {
                        ModelState.AddModelError(nameof(rental.RentalStartDate), "Start date cannot be in the past.");
                    }
                    else if (rental.RentalEndDate <= rental.RentalStartDate)
                    {
                        ModelState.AddModelError(nameof(rental.RentalEndDate), "End date should be after the start date.");
                    }
                    else
                    {
                        context.Rentals.Add(rental);
                        context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid user information";
                    return View(rental);
                }
            }

            // If ModelState is not valid, reload the form with dropdown data and the user's entered data
            PopulateDropdownsInViewBag(rental);
            return View(rental);
        }

        // Méthode privée pour charger les données des dropdowns dans le ViewBag
        private void PopulateDropdownsInViewBag(Rental rental)
        {
            var persons = context.Persons.ToList();
            var vehicles = context.Vehicles.ToList();

            ViewBag.Persons = new SelectList(persons, "PersonId", "FirstName", rental.PersonId);
            ViewBag.Vehicles = new SelectList(vehicles, "VehicleId", "PlateNumber", rental.VehicleId);
        }





        // GET: RentalController/Edit/5
        private ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = context.Rentals
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.RentallD == id);

            if (rental == null)
            {
                return NotFound();
            }

            ViewBag.Persons = new SelectList(context.Persons, "PersonId", "FirstName", rental.PersonId); // Populate dropdown for Persons
            ViewBag.Vehicles = new SelectList(context.Vehicles, "VehicleId", "Model", rental.VehicleId); // Populate dropdown for Vehicles

            return View(rental);
        }

        // POST: RentalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Rental rental)
        {
            if (id != rental.RentallD)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(rental);
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.RentallD))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Persons = new SelectList(context.Persons, "PersonId", "FirstName", rental.PersonId); // Populate dropdown for Persons if validation fails
            ViewBag.Vehicles = new SelectList(context.Vehicles, "VehicleId", "Model", rental.VehicleId); // Populate dropdown for Vehicles if validation fails

            return View(rental);
        }

        private bool RentalExists(int id)
        {
            return context.Rentals.Any(r => r.RentallD == id);
        }


        // GET: RentalController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = context.Rentals
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.RentallD == id);

            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: RentalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var rental = context.Rentals.Find(id);
            if (rental == null)
            {
                return NotFound();
            }

            context.Rentals.Remove(rental);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
