using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rooms;

public class RoomService : BaseService, IRoomService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetAvailableRoomsRequest> _validator;

    public RoomService(IApplicationDbContext context, IValidator<GetAvailableRoomsRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    public Result<IEnumerable<RoomDto>> GetAvailableRooms(GetAvailableRoomsRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return GetFailureResult<IEnumerable<RoomDto>>(validationResult);
        }

        // Get rooms that meet guest capacity requirement
        var rooms = _context.Rooms
            .Where(r => r.Capacity >= request.Guests)
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Type = r.Type,
                Capacity = r.Capacity,
                HotelId = r.HotelId,
                HotelName = r.Hotel.Name
            });

        // Get room IDs that are already booked for the given dates
        var unavailableRoomIds = _context.Bookings
            .Where(b => b.CheckInDate < request.CheckOutDate && b.CheckOutDate > request.CheckInDate)
            .Select(b => b.RoomId)
            .ToList();

        return Result<IEnumerable<RoomDto>>.SuccessResult(rooms.Where(r => !unavailableRoomIds.Contains(r.Id)).ToList());
    }
}

