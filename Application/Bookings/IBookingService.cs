using Application.Common.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings;

public interface IBookingService
{
    Result<Booking> CreateBooking(CreateBookingRequest request);
    Booking? GetBooking(string bookingNumber);
    void CancelBooking(string bookingNumber);
}
