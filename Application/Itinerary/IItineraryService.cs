using Application.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public interface IItineraryService
{
    public Task<Result<ItineraryResponse?>>  GenerateItineraryAsync(ItineraryRequest request, CancellationToken cancellationToken);
}
