using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rooms;

public record GetAvailableRoomsRequest(DateTime CheckInDate, DateTime CheckOutDate, int Guests);
