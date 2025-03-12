using Application.Common.Interfaces;
using Application.Common.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Itinerary;

public class ItineraryService : BaseService, IItineraryService
{
    private readonly ILogger<ItineraryService> _logger;
    private readonly IAIItineraryGeneratorService _itineraryGeneratorService;
    private readonly IValidator<ItineraryRequest> _validator;
    private readonly IApplicationDbContext _applicationDbContext;

    public ItineraryService(ILogger<ItineraryService> logger, IAIItineraryGeneratorService itineraryGeneratorService, IValidator<ItineraryRequest> validator, IApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _itineraryGeneratorService = itineraryGeneratorService;
        _validator = validator;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Result<ItineraryResponse?>> GenerateItineraryAsync(ItineraryRequest request, CancellationToken cancellationToken)
    {        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return GetFailureResult<ItineraryResponse?>(validationResult);
        }

        ItineraryResponse response =  await _itineraryGeneratorService.GenerateItineraryAsync(request, cancellationToken);
        _logger.LogInformation("GenerateItineraryAsync responded");
        var ItineraryRecord = new Domain.Entities.Itinerary
        {
            Destination = request.Destination,
            GeneratedAt = DateTime.UtcNow,
            ItineraryJson = JsonConvert.SerializeObject(response)
        };
        _applicationDbContext.Itineraries.Add(ItineraryRecord);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("ItineraryRecord saved");

        return Result<ItineraryResponse?>.SuccessResult(response);
    }
}
