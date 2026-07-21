using Application.DTOs.DiscountDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddDiscountDtoValidator : AbstractValidator<AddDiscountDto>
    {
        public AddDiscountDtoValidator()
        {
            RuleFor(x => x.DiscountAmount).GreaterThan(0);
        }
    }
}
