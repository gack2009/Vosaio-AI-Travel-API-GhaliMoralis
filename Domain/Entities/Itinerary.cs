using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Itinerary: BaseEntity
{
    public required string Destination { get; set; }
    public required string ItineraryJson { get; set; } 
    public DateTime GeneratedAt { get; set; }
}
