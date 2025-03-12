using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public class ActivityDto
{
    public string Time { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal EstimatedCost { get; set; }
}
