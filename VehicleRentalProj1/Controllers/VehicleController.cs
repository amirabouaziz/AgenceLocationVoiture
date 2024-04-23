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
            var vehicles = context.Vehicles.ToList();
            return View(vehicles);
        }

        // GET: VehicleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VehicleController/Create
        public ActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(context.Locations, "LocationId", "LocationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Générer un nom de fichier unique pour l'image (par exemple, en utilisant GUID)
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // Chemin physique pour sauvegarder l'image dans wwwroot/Images (exemple)
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", uniqueFileName);

                    // Enregistrer physiquement le fichier sur le serveur
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    // Enregistrer le chemin du fichier dans la propriété ImagePath du véhicule
                    vehicle.ImagePath = "Images/" + uniqueFileName; // Ou enregistrez le chemin complet si nécessaire
                }

                context.Add(vehicle);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Si le modèle n'est pas valide, retournez la vue avec le modèle pour afficher les erreurs
            ViewData["LocationId"] = new SelectList(context.Locations, "LocationId", "LocationName", vehicle.LocationId);
            return View(vehicle);
        }




        // GET: VehicleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle model, IFormFile imageFile)
        {
            if (id != model.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVehicle = await context.Vehicles.FindAsync(id);

                    if (existingVehicle == null)
                    {
                        return NotFound();
                    }

                    // Update other vehicle properties
                    existingVehicle.Model = model.Model;
                    existingVehicle.PlateNumber = model.PlateNumber;
                    existingVehicle.LocationId = model.LocationId;

                    // Handle image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Process the uploaded image - Save to directory, database, or cloud storage
                        // For instance, you might save the image to the wwwroot folder
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageFile.FileName);

                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Update the ImagePath property in the database
                        existingVehicle.ImagePath = "/images/" + imageFile.FileName;
                    }

                    // Save changes to the database
                    await context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    // Handle errors
                    return View(model);
                }
            }
            return View(model);
        }


        // GET: VehicleController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await context.Vehicles
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
