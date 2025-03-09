using Application.Bookings;
using Application.Common.Interfaces;
using Application.Hotels;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Services;

public class HotelServiceTests
{
    private readonly Mock<IApplicationDbContext> _mockDbContext;
    private readonly HotelService _hotelService;

    public HotelServiceTests()
    {
        _mockDbContext = new Mock<IApplicationDbContext>();
        _hotelService = new HotelService(_mockDbContext.Object, new GetHotelsRequestValidator());
    }

    [Fact]
    public void GetHotels_ShouldReturnListOfHotels()
    {
        // Arrange
        Guid hotelAId = new Guid("D5AF7224-C887-480F-BE9D-C0F91F100EBB");
        Guid hotelBId = new Guid("3CECF919-4A41-4832-9903-3EBE24C3FF4A");
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = hotelAId, Name = "Hotel A",
                Rooms = new List<Room>
                {
                    new Room { Id = new Guid("8248F744-9139-45F7-9220-73A9DE5E4B01"), HotelId = hotelAId, Type = RoomType.Single, Capacity = 1 },
                    new Room { Id = new Guid("86CD9C12-FD09-4285-BD7A-023C92BD004F"), HotelId = hotelAId, Type = RoomType.Double, Capacity = 2 }
                }
            },
            new Hotel
            {
                Id = new Guid("3CECF919-4A41-4832-9903-3EBE24C3FF4A"), Name = "Hotel B",
                Rooms = new List<Room>
                {
                    new Room { Id = new Guid("86CD9C12-FD09-4285-BD7A-023C92BD004F"), HotelId = hotelBId, Type = RoomType.Deluxe, Capacity = 4 }
                }
            }
        }.AsQueryable();
        var mockSet = Arrange.CreateMockDbSet(hotels);
        _mockDbContext.Setup(c => c.Hotels).Returns(mockSet.Object);

        // Act
        var result = _hotelService.GetHotels();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
