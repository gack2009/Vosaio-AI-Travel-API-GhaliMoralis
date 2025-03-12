using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public class ItineraryRequest
{
    public required string Destination { get; set; }
    public required DateTime[] TravelDates { get; set; }//I would recommend using 2 properties instead of an array e.g. startDate abd endDate
    public decimal Budget { get; set; }
    public string[]? Interests { get; set; }
}
