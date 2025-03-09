using Application.Bookings;
using Application.Common.Interfaces;
using Application.Rooms;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Xml.Linq;
using System;
using Domain.Enums;
using FluentValidation;
using Application.UnitTests.Common;

namespace Application.UnitTests.Services;

public class RoomServiceTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly RoomService _roomService;

    public RoomServiceTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _roomService = new RoomService(_mockContext.Object, new GetAvailableRoomsRequestValidator());
    }

    [Fact]
    public void GetAvailableRooms_ShouldReturnListOfAvailableRooms()
    {

        // Arrange
        var hotelId = new Guid("6C075230-59D3-4EAC-A3CD-2C3550F4D562");
        var hotel = new Hotel{ Id = hotelId, Name = "test hotel" };
        var roomsList = new List<Room>
        {
            new Room { Id = new Guid("EBC12D7E-17C7-4F09-BA56-06CEEB9C01CB"), Type = RoomType.Single, Capacity = 1 , HotelId = hotelId, Hotel = hotel},
            new Room { Id = new Guid("8E3661F8-26A0-45E5-A139-EE080BD03A64"), Type = RoomType.Double, Capacity = 2 , HotelId = hotelId, Hotel = hotel}
        }.AsQueryable();
        var bookings = new List<Booking>().AsQueryable();
        var Hotels = new List<Hotel>
        {
            hotel
        }.AsQueryable();

        var mockSet = Arrange.CreateMockDbSet(roomsList);
        var mockBookingSet = Arrange.CreateMockDbSet(bookings);
        var mockHotelSet = Arrange.CreateMockDbSet(Hotels);
        _mockContext.Setup(c => c.Rooms).Returns(mockSet.Object);
        _mockContext.Setup(c => c.Bookings).Returns(mockBookingSet.Object);
        _mockContext.Setup(c => c.Hotels).Returns(mockHotelSet.Object);
        var request = new GetAvailableRoomsRequest(DateTime.Now, DateTime.Now.AddDays(2), 1);

        // Act
        var result = _roomService.GetAvailableRooms(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
    }
}
