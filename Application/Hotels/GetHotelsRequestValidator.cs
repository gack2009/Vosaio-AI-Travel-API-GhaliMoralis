using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hotels;

public class GetHotelsRequestValidator : AbstractValidator<GetHotelsRequest>
{
    public GetHotelsRequestValidator()
    {
        RuleFor(x => x.name)
            .NotEmpty()
            .NotNull()
            .WithName("Name");
    }
}
