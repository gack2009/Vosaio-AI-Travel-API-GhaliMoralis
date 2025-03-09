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

namespace Application.Hotels;

public class HotelService : BaseService, IHotelService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetHotelsRequest> _validator;

    public HotelService(IApplicationDbContext context, IValidator<GetHotelsRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    public Result<HotelDto?> GetHotel(GetHotelsRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return GetFailureResult<HotelDto?>(validationResult);
        }
        HotelDto? hotel = _context.Hotels
            .Where(h => h.Name == request.name)
            .Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Rooms = h.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Type = r.Type,
                    Capacity = r.Capacity
                }).ToList()
            })
            .SingleOrDefault();

        return Result<HotelDto?>.SuccessResult(hotel);
    }

    public List<HotelDto> GetHotels()
    {
        var hotels = _context.Hotels
                .Select(h => new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Rooms = h.Rooms.Select(r => new RoomDto
                    {
                        Id = r.Id,
                        Type = r.Type,
                        Capacity = r.Capacity
                    }).ToList()
                }).ToList();
        return hotels;
    }
}
