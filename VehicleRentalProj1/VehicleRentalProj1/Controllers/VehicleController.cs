using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using VehicleRentalProj1.Models;
using Microsoft.AspNetCore.Hosting;


namespace VehicleRentalProj1.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehiclesRent1Context context;
        private readonly IWebHostEnvironment _hostEnvironment;

      
        public VehicleController(VehiclesRent1Context context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            _hostEnvironment = hostEnvironment; // Injection de l'IWebHostEnvironment
        }
        // GET: VehicleController
        public ActionResult Index()
        {
            List<Vehicle> vehicle = context.Vehicles.ToList();
            return View(vehicle);
        }

        // GET: VehicleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VehicleController/Create
        public ActionResult Create()
        {
            ViewBag.idselect = context.Locations.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle )
        {
            try
            {
                context.Add(vehicle);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

            // Si le modèle n'est pas valide, retournez la vue avec le modèle pour afficher les erreurs
            return View(vehicle);
        }




        // GET: VehicleController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.idselect = new SelectList(context.Locations, "LocationId", "LocationName", vehicle.LocationId);
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(vehicle); // Tracks the changes made to the vehicle object

                    await context.SaveChangesAsync(); // Commits the changes to the database

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
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

            ViewBag.idselect = new SelectList(context.Locations, "LocationId", "LocationName", vehicle.LocationId);
            return View(vehicle);
        }



        private bool VehicleExists(int id)
        {
            return context.Vehicles.Any(e => e.VehicleId == id);
        }


        // GET: VehicleController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await context.Vehicles
                .Include(v => v.Location) // Include related Location entity
                .FirstOrDefaultAsync(m => m.VehicleId == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: VehicleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
