using Application.Common.Interfaces;
using Application.Rooms;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class DBSetupController : ControllerBase
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _appConfig;

    public DBSetupController(IApplicationDbContext dbContext, IConfiguration appConfig)
    {
        _dbContext = dbContext;
        _appConfig = appConfig;
    }

    [HttpGet("SeedDB")]
    public IActionResult SeedDb()
    {
        try
        {
            if (!_dbContext.Hotels.Any())
            {
                List<Hotel> hotels = new List<Hotel>
                {
                    new Hotel{
                        Name = "DoubleTree Hotel",
                        Rooms = new List<Room> {
                            new Room { Type = RoomType.Single, Capacity = 1 },
                            new Room { Type = RoomType.Single, Capacity = 1 },
                            new Room { Type = RoomType.Double, Capacity = 2 },
                            new Room { Type = RoomType.Double, Capacity = 3 },
                            new Room { Type = RoomType.Deluxe, Capacity = 4 },
                            new Room { Type = RoomType.Deluxe, Capacity = 5 },
                        }
                    },
                    new Hotel{
                        Name = "Mega Hotel",
                        Rooms = new List<Room> {
                            new Room { Type = RoomType.Single, Capacity = 1 },
                            new Room { Type = RoomType.Single, Capacity = 1 },
                            new Room { Type = RoomType.Single, Capacity = 1 },
                            new Room { Type = RoomType.Double, Capacity = 2 },
                            new Room { Type = RoomType.Double, Capacity = 2 },
                            new Room { Type = RoomType.Double, Capacity = 2 }
                        }
                    },
                    new Hotel{
                        Name = "Golden Beach Hotel",
                        Rooms = new List<Room> {
                            new Room { Type = RoomType.Double, Capacity = 2 },
                            new Room { Type = RoomType.Deluxe, Capacity = 5 },
                            new Room { Type = RoomType.Deluxe, Capacity = 4 },
                            new Room { Type = RoomType.Deluxe, Capacity = 5 },
                            new Room { Type = RoomType.Deluxe, Capacity = 6 },
                            new Room { Type = RoomType.Deluxe, Capacity = 6 }
                        }
                    },
                };
                _dbContext.Hotels.AddRange(hotels);
                _dbContext.SaveChanges();
            }
            return Ok(new { message = "Database seeded successfully." });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet("ResetDB")]
    public IActionResult ResetDb()
    {
        try
        {
            _dbContext.Bookings.RemoveRange(_dbContext.Bookings);
            _dbContext.Rooms.RemoveRange(_dbContext.Rooms);
            _dbContext.Hotels.RemoveRange(_dbContext.Hotels);
            _dbContext.SaveChanges();
            return Ok(new { message = "Database reset successfully." });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }
}

