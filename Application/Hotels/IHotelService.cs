using Application.Common.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hotels;

public interface IHotelService
{
    Result<HotelDto?> GetHotel(GetHotelsRequest request);
    List<HotelDto> GetHotels();
}
