using Application.Itinerary;
using Infrastructure.AIService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Infrastructure.IntegrationTest;

public class OpenAIItineraryGeneratorTests
{
    [Fact]
    public async Task GenerateItineraryAsync_ValidResponse_ReturnsParsedItineraryResponse()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Create a sample ItineraryResponse.
        var sampleItineraryResponse = new ItineraryResponse
        {
            Destination = "Tokyo",
            StartDate = new DateTime(2025, 6, 1),
            EndDate = new DateTime(2025, 6, 10),
            TotalEstimatedCost = 2000,
            DayPlans = new List<DayPlanDto>()
        };

        string itineraryJson = JsonConvert.SerializeObject(sampleItineraryResponse, Formatting.Indented);
        string contentWithCodeFences = $"```json\n{itineraryJson}\n```";
        var fakeOpenAiResponse = new
        {
            choices = new[]
            {
                new {
                    message = new {
                        content = contentWithCodeFences
                    }
                }
            }
        };
        string fakeApiResponse = JsonConvert.SerializeObject(fakeOpenAiResponse);
        // Create a fake HttpResponseMessage with HTTP 200 OK.
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(fakeApiResponse, Encoding.UTF8, "application/json")
        };

        // Create a fake HttpMessageHandler and HttpClient.
        var fakeHandler = new FakeHttpMessageHandler(httpResponseMessage);
        var httpClient = new HttpClient(fakeHandler);

        var inMemorySettings = new Dictionary<string, string>
        {
            {"OpenAI:ApiKey", "test-api-key"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var generator = new OpenAIItineraryGenerator(httpClient, configuration);
        var request = new ItineraryRequest
        {
            Destination = "Tokyo",
            TravelDates = new DateTime[] { new DateTime(2025, 6, 1), new DateTime(2025, 6, 10) },
            Budget = 2000,
            Interests = new[] { "history", "food", "adventure" }
        };

        // Act
        ItineraryResponse result = await generator.GenerateItineraryAsync(request, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sampleItineraryResponse.Destination, result.Destination);
        Assert.Equal(sampleItineraryResponse.StartDate, result.StartDate);
        Assert.Equal(sampleItineraryResponse.EndDate, result.EndDate);
        Assert.Equal(sampleItineraryResponse.TotalEstimatedCost, result.TotalEstimatedCost);
    }

    [Fact]
    public async Task GenerateItineraryAsync_WithRealApi_ReturnsValidItineraryResponse()
    {
        // Arrange
        string filePath = Path.Combine("..", "..", "..", "..", "WebApplication1", "appsettings.json");
        string json = File.ReadAllText(filePath);
        AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
        var configDict = new Dictionary<string, string>
        {
            { "OpenAI:ApiKey", appSettings.OpenAI.ApiKey }
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        using var httpClient = new HttpClient();
        var generator = new OpenAIItineraryGenerator(httpClient, configuration);
        var request = new ItineraryRequest
        {
            Destination = "Tokyo",
            TravelDates = new DateTime[] { new DateTime(2025, 6, 1), new DateTime(2025, 6, 10) },
            Budget = 2000,
            Interests = new[] { "history", "food", "adventure" }
        };

        // Act
        ItineraryResponse response = await generator.GenerateItineraryAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Tokyo", response.Destination);
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}

public class AppSettings
{
    public OpenAISettings OpenAI { get; set; }
}

public class OpenAISettings
{
    public string ApiKey { get; set; }
}
