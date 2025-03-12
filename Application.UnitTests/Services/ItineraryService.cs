using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Itinerary;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.ObjectModel;
using Domain.Entities;

namespace Application.UnitTests.Services;

public class ItineraryServiceTests
{
    private readonly Mock<ILogger<ItineraryService>> _mockLogger;
    private readonly Mock<IAIItineraryGeneratorService> _mockGeneratorService;
    private readonly Mock<IValidator<ItineraryRequest>> _mockValidator;
    private readonly Mock<IApplicationDbContext> _mockDbContext;
    private readonly ItineraryService _service;

    public ItineraryServiceTests()
    {
        _mockLogger = new Mock<ILogger<ItineraryService>>();
        _mockGeneratorService = new Mock<IAIItineraryGeneratorService>();
        _mockValidator = new Mock<IValidator<ItineraryRequest>>();
        _mockDbContext = new Mock<IApplicationDbContext>();
        _mockDbContext.Setup(db => db.Itineraries).Returns(DbSetFakeHelper.GetFakeDbSet(new List<Domain.Entities.Itinerary>()));
        _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

        _service = new ItineraryService(
            _mockLogger.Object,
            _mockGeneratorService.Object,
            _mockValidator.Object,
            _mockDbContext.Object);
    }

    [Fact]
    public async Task GenerateItineraryAsync_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new ItineraryRequest
        {
            Destination = "Tokyo",
            TravelDates = new DateTime[] { new DateTime(2025, 6, 1), new DateTime(2025, 6, 10) },
            Budget = 2000,
            Interests = new[] { "history", "food", "adventure" }
        };

        // Setup the validator to return a valid result.
        var validResult = new ValidationResult();
        _mockValidator
            .Setup(v => v.ValidateAsync(request, cancellationToken))
            .ReturnsAsync(validResult);

        // Setup the AI itinerary generator to return a sample itinerary response.
        var sampleResponse = new ItineraryResponse
        {
            Destination = request.Destination,
            StartDate = request.TravelDates[0],
            EndDate = request.TravelDates[1],
            TotalEstimatedCost = 2000,
            DayPlans = new List<DayPlanDto>()
        };

        _mockGeneratorService
            .Setup(s => s.GenerateItineraryAsync(request, cancellationToken))
            .ReturnsAsync(sampleResponse);

        // Act
        var result = await _service.GenerateItineraryAsync(request, cancellationToken);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Tokyo", result.Data.Destination);

        // Verify that the repository Add and SaveChangesAsync methods were called once.
        _mockDbContext.Verify(db => db.Itineraries.Add(It.IsAny<Domain.Entities.Itinerary>()), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GenerateItineraryAsync_InvalidRequest_ReturnsFailureResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new ItineraryRequest
        {
            Destination = "", // Invalid because it's empty.
            TravelDates = new DateTime[] { new DateTime(2025, 6, 1), new DateTime(2025, 6, 10) },
            Budget = 2000,
            Interests = new[] { "history", "food", "adventure" }
        };

        // Setup the validator to return an invalid result.
        var failure = new ValidationFailure("Destination", "Destination is required");
        var invalidResult = new ValidationResult(new[] { failure });
        _mockValidator
            .Setup(v => v.ValidateAsync(request, cancellationToken))
            .ReturnsAsync(invalidResult);

        // Act
        var result = await _service.GenerateItineraryAsync(request, cancellationToken);

        // Assert that the result is a failure.
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Destination is required", string.Join(" ", result.Errors["Destination"]));

        // Verify that neither the AI service nor the DbContext were called.
        _mockGeneratorService.Verify(s => s.GenerateItineraryAsync(It.IsAny<ItineraryRequest>(), cancellationToken), Times.Never);
        _mockDbContext.Verify(db => db.Itineraries.Add(It.IsAny<Domain.Entities.Itinerary>()), Times.Never);
        _mockDbContext.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Never);
    }
}

public static class DbSetFakeHelper
{
    public static DbSet<T> GetFakeDbSet<T>(List<T> list) where T : class
    {
        var queryable = list.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        // When Add is called, add the item to the underlying list.
        mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(list.Add);

        return mockSet.Object;
    }
}