using Application.Common.Services;
using Application.Hotels;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    public HotelsController(IHotelService hotelService) => _hotelService = hotelService;

    [HttpGet]
    public ActionResult<IEnumerable<HotelDto>> Get()
    {
        return Ok(_hotelService.GetHotels());
    }

    [HttpGet("{name}")]
    public ActionResult<HotelDto?> Get([FromRoute] GetHotelsRequest request)
    {
        Result<HotelDto?> result = _hotelService.GetHotel(request);
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Data);
    }
}
