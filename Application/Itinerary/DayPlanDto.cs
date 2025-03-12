using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public class DayPlanDto
{
    public DateTime Date { get; set; }
    public List<ActivityDto> Activities { get; set; }
    public HotelDto Hotel { get; set; }
    public List<string> Restaurants { get; set; }
}
