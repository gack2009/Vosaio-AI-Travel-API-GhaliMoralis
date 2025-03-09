using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rooms;

public class GetAvailableRoomsRequestValidator : AbstractValidator<GetAvailableRoomsRequest>
{
    public GetAvailableRoomsRequestValidator()
    {
        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Check-in date must be today or later.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.Guests)
            .GreaterThan(0)
            .WithMessage("Guest count must be at least 1.");
    }
}
