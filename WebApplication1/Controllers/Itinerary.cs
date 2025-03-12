using Application.Common.Services;
using Application.Itinerary;
using Microsoft.AspNetCore.Mvc;

namespace AITravelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryController : ControllerBase
{
    private readonly IItineraryService _itineraryService;
    private readonly ILogger<ItineraryController> _logger;
    public ItineraryController(IItineraryService itineraryService, ILogger<ItineraryController> logger)
    {
        _itineraryService = itineraryService;
        _logger = logger;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ItineraryResponse>> Generate([FromBody] ItineraryRequest request, CancellationToken cancellationToken)
    {
        Result<ItineraryResponse?> generatedItinerary = await _itineraryService.GenerateItineraryAsync(request, cancellationToken);
        if (!generatedItinerary.Success)
        {
            _logger.LogError("Service retured an error at {Generate}", nameof(Generate));
            return BadRequest(generatedItinerary.Errors);
        }
        return Ok(generatedItinerary.Data);
    }
}
