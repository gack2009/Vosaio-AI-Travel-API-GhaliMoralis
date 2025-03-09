using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings;

public record CreateBookingRequest(Guid RoomId, string MainGuestFullName, DateTime CheckInDate, DateTime CheckOutDate, int Guests);
