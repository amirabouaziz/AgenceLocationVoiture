using System;
using System.Collections.Generic;

namespace VehicleRentalProj1.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string email { get; set; }=null!;
    public string password { get; set; }=null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
