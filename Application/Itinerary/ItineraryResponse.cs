using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public class ItineraryResponse
{
    public string Destination { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalEstimatedCost { get; set; }
    public List<DayPlanDto> DayPlans { get; set; }
}
