using System;
using System.Collections.Generic;

namespace VehicleRentalProj1.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string Model { get; set; } = null!;

    public string PlateNumber { get; set; } = null!;
    public string ImagePath { get; set; }

    public int LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

}
