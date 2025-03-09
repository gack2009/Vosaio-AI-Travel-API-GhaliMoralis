using Application.Rooms;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AvailableRoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public AvailableRoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RoomDto>> GetAvailableRooms([FromQuery] GetAvailableRoomsRequest getAvailableRoomsRequest)
    {
        var availableRooms = _roomService.GetAvailableRooms(getAvailableRoomsRequest);
        if (!availableRooms.Success)
        {
            return BadRequest(availableRooms.Errors);
        }
        return Ok(availableRooms.Data);
    }
}

