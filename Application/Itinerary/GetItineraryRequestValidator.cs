using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Itinerary;

public class GetItineraryRequestValidator : AbstractValidator<ItineraryRequest>
{
    public GetItineraryRequestValidator()
    {
        RuleFor(x => x.Destination)
            .NotEmpty();//TODO: Still need to improve this validation to check if this is a country name

        RuleFor(x => x.TravelDates)
            .NotEmpty()
            .Must(x => x.Any() == true && x.Count() == 2).WithMessage("The Travel Dates must have two dates, travel start date and travel end date")
            .Must(x => x[0] <= x[1]).WithMessage("The travel end date should be on same day or after the travel start date");

        RuleFor(x => x.Budget)
            .NotEmpty()
            .GreaterThan(0);

        //I don't think interests require validation. not sure if it should be nullable or not
    }
}
