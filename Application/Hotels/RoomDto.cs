using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hotels;

public class RoomDto
{
    public Guid Id { get; set; }
    public RoomType Type { get; set; } 
    public int Capacity { get; set; }
}
