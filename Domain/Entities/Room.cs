using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Room: BaseEntity
{
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public RoomType Type { get; set; } // Single, Double, Deluxe
    public int Capacity { get; set; }
}
