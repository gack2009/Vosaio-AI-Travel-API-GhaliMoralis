using AITravelAPI.Controllers;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Itinerary;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AITravelAPI.UnitTest.Controller;

public class ItineraryControllerTests
{
    private readonly Mock<IItineraryService> _mockService;
    private readonly ItineraryController _controller;
    private readonly ILogger<ItineraryController> _logger;

    public ItineraryControllerTests()
    {
        // Setup the mocked AI service.
        _mockService = new Mock<IItineraryService>();

        // Create a logger (you could also use a NullLogger for tests).
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<ItineraryController>();

        // Instantiate the controller with the mocked service, and logger.
        _controller = new ItineraryController(_mockService.Object, _logger);
    }

    [Fact]
    public async Task GenerateItineraryAsync_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new ItineraryRequest
        {
            Destination = "Tokyo",
            TravelDates = [new DateTime(2025, 06, 01), new DateTime(2025, 06, 10)],
            Budget = 2000,
            Interests = ["history", "food", "adventure"]
        };

        var fakeItineraryResult = Result<ItineraryResponse?>.SuccessResult(new ItineraryResponse
        {
            Destination = request.Destination,
            StartDate = request.TravelDates[0],
            EndDate = request.TravelDates[1],
            TotalEstimatedCost = 2000,
            DayPlans = new List<DayPlanDto>()
        });

        _mockService
            .Setup(s => s.GenerateItineraryAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeItineraryResult);

        // Act
        var result = await _controller.Generate(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseValue = Assert.IsType<ItineraryResponse?>(okResult.Value);
        Assert.Equal("Tokyo", responseValue.Destination);
    }

    [Fact]
    public async Task GenerateItineraryAsync_MissingDestination_ReturnsBadRequest()
    {
        // Arrange: Empty destination should be invalid.
        var request = new ItineraryRequest
        {
            Destination = "",
            TravelDates = new DateTime[] { new DateTime(2025, 06, 01), new DateTime(2025, 06, 10) },
            Budget = 2000,
            Interests = new string[] { "history", "food", "adventure" }
        };
        var fakeItineraryResult = Result<ItineraryResponse?>.FailureResult(new Dictionary<string, string[]> { { nameof(ItineraryRequest.Destination), ["Cannot be null"] } });

        _mockService
            .Setup(s => s.GenerateItineraryAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeItineraryResult);

        // Act
        var result = await _controller.Generate(request, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var errors = badRequest.Value as IDictionary<string, string[]>;
        Assert.Contains(nameof(ItineraryRequest.Destination), errors);
    }
}
