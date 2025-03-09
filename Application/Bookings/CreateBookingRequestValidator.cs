using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    private readonly IApplicationDbContext _context;
    public CreateBookingRequestValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.RoomId)
            .NotEmpty()
            .Must(RoomExists).WithMessage("Room does not exist.");

        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Check-in date must be today or later.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.Guests)
            .GreaterThan(0)
            .WithMessage("At least one guest is required.")
            .Must((request, guests) => RoomCapacityIsValid(request.RoomId, guests))
            .WithMessage("Guest count exceeds room capacity.");

        RuleFor(x => x)
            .Must(request => RoomIsAvailable(request.RoomId, request.CheckInDate, request.CheckOutDate))
            .WithMessage("Room is already booked for these dates.")
            .WithName("CreateBookingRequest");
    }

    private bool RoomExists(Guid roomId)
    {
        return _context.Rooms.Any(r => r.Id == roomId);
    }

    private bool RoomCapacityIsValid(Guid roomId, int guests)
    {
        var room = _context.Rooms.Find(roomId);
        return room != null && guests <= room.Capacity;
    }

    private bool RoomIsAvailable(Guid roomId, DateTime checkIn, DateTime checkOut)
    {
        return !_context.Bookings.Any(b => b.RoomId == roomId &&
            (b.CheckInDate < checkOut && b.CheckOutDate > checkIn));
    }
}
