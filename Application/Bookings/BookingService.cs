using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings;

public class BookingService : BaseService, IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateBookingRequest> _validator;

    public BookingService(IApplicationDbContext context, IValidator<CreateBookingRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    private string GenerateBookingNumber(CreateBookingRequest request)
    {
        //This method can be improved
        string prefix = $"HTL";
        string datePart = DateTime.UtcNow.ToString("ddMMyy");
        string randomCode = GenerateRandomCode(4);

        return $"{prefix}{request.Guests}{request.RoomId.ToString().Substring(0,2)}" +
            $"{request.MainGuestFullName.Substring(0, 2)}{datePart}{randomCode}";
    }

    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public Result<Booking> CreateBooking(CreateBookingRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return GetFailureResult<Booking>(validationResult);
        }

        var booking = new Booking
        {
            BookingNumber = GenerateBookingNumber(request),
            RoomId = request.RoomId,
            MainGuestFullName = request.MainGuestFullName,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Guests = request.Guests
        };

        _context.Bookings.Add(booking);
        _context.SaveChanges();
        return Result<Booking>.SuccessResult(booking);
    }

    public Booking? GetBooking(string bookingNumber)
    {
        return _context.Bookings.FirstOrDefault(b => b.BookingNumber == bookingNumber);
    }

    public void CancelBooking(string bookingNumber)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.BookingNumber == bookingNumber);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }
    }
}
