using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hotels;

public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<RoomDto> Rooms { get; set; }
}
