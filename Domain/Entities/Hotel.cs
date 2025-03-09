using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Hotel: BaseEntity
{
    public required string Name { get; set; }
    //public int NumberOfRooms {  get; set; }
    public List<Room>? Rooms { get; set; }
}
