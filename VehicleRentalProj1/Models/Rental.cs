using System;
using System.Collections.Generic;

namespace VehicleRentalProj1.Models;

public partial class Rental
{
    public int RentallD { get; set; }

    public int PersonId { get; set; }

    public DateTime RentalEndDate { get; set; }
    public DateTime RentalStartDate { get; set; }


    public int VehicleId { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
