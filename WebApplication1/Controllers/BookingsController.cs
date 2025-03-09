using Application.Bookings;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService) => _bookingService = bookingService;

    [HttpPost]
    public ActionResult<Booking> CreateBooking([FromForm] CreateBookingRequest request)
    {
        var newBookingResult = _bookingService.CreateBooking(request);
        if (!newBookingResult.Success)
        {
            return BadRequest(newBookingResult.Errors);
        }
        return CreatedAtAction(nameof(GetBooking), new { bookingNumber = newBookingResult.Data?.BookingNumber }, newBookingResult.Data);
    }

    [HttpGet("{bookingNumber}")]
    public ActionResult<Booking> GetBooking([FromRoute] string bookingNumber)
    {
        var booking = _bookingService.GetBooking(bookingNumber);
        return booking == null ? NotFound() : Ok(booking);
    }

    [HttpDelete("{bookingNumber}")]
    public IActionResult CancelBooking([FromRoute] string bookingNumber)
    {
        _bookingService.CancelBooking(bookingNumber);
        return Ok();
    }
}
