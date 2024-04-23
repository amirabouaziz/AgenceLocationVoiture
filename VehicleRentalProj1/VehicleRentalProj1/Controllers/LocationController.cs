using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VehicleRentalProj1.Models;

namespace VehicleRentalProj1.Controllers
{
    public class LocationController : Controller
        
    {
        private readonly VehiclesRent1Context context;

        public LocationController(VehiclesRent1Context context)
        {
            this.context = context;
        }
       
        // GET: LocationController
        public ActionResult Index()
        {
            var locations = context.Locations.Include(l => l.Vehicles).ToList();
            return View(locations);
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleRentalProj1.Models.Location location1) // Spécifier explicitement l'espace de noms du modèle
        {
            if (ModelState.IsValid)
            {               
                 context.Locations.Add(location1);
                 context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(location1);
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = context.Locations.Find(id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VehicleRentalProj1.Models.Location editedLocation)
        {
            if (id != editedLocation.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Attach(editedLocation);
                    context.Entry(editedLocation).Property("LocationName").IsModified = true;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(editedLocation.LocationId))
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
            return View(editedLocation);
        }
        private bool LocationExists(int id)
        {
            return context.Locations.Any(e => e.LocationId == id);
        }

        // GET: LocationController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: LocationController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            context.Locations.Remove(location);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
