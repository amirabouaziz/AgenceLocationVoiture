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
    var persons = context.Persons.ToList(); // Check if this returns any records
    var vehicles = context.Vehicles.ToList(); // Check if this returns any records

    ViewBag.Persons = new SelectList(persons, "PersonId", "FirstName");
    ViewBag.Vehicles = new SelectList(vehicles, "VehicleId", "PlateNumber");

    return View();
}

// POST: RentalController/Create
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Rental rental)
{
    if (ModelState.IsValid)
    {
        // Perform validation and save to the database
        context.Rentals.Add(rental);
        context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    var persons = context.Persons.ToList(); // Fetch data again in case of validation error
    var vehicles = context.Vehicles.ToList(); // Fetch data again in case of validation error

    ViewBag.Persons = new SelectList(persons, "PersonId", "FirstName", rental.PersonId); // Populate dropdown for Persons if validation fails
    ViewBag.Vehicles = new SelectList(vehicles, "VehicleId", "PlateNumber", rental.VehicleId); // Populate dropdown for Vehicles if validation fails

    return View(rental);
}


        // GET: RentalController/Edit/5
        public ActionResult Edit(int id)
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
