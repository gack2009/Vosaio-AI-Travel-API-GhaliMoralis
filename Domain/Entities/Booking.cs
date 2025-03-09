using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Booking: BaseEntity
{
    public required string BookingNumber { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; } 
    public required string MainGuestFullName { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int Guests { get; set; }
}
