using Application.Bookings;
using Application.Common.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Services;

public class BookingServiceTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _bookingService = new BookingService(_mockContext.Object, new CreateBookingRequestValidator(_mockContext.Object));
    }

    [Fact]
    public void CreateBooking_ShouldReturnBooking_WhenValidRequest()
    {
        // Arrange
        Guid roomId = new Guid("8BE86757-F019-4BD3-9B99-E53026099595");
        var rooms = new List<Room> { new Room { Id = roomId, Type = RoomType.Single, Capacity = 1 } }.AsQueryable();
        var mockSet = Arrange.CreateMockDbSet(rooms);
        var mockBookingSet = Arrange.CreateMockDbSet(new List<Booking>().AsQueryable());
        _mockContext.Setup(c => c.Rooms).Returns(mockSet.Object);
        _mockContext.Setup(c => c.Bookings).Returns(mockBookingSet.Object);
        _mockContext.Setup(c => c.SaveChanges()).Returns(1);
        _mockContext.Setup(c => c.Rooms.Find(roomId)).Returns(rooms.First());
        var request = new CreateBookingRequest(roomId, "Full Name", DateTime.Now, DateTime.Now.AddDays(2), 1);

        // Act
        var result = _bookingService.CreateBooking(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(roomId, result.Data.RoomId);
    }
}
