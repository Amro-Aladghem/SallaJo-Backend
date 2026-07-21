using Application.DTOs.OfferDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddOfferDtoValidator :AbstractValidator<AddOfferDto>
    {
        public AddOfferDtoValidator()
        {
            RuleFor(x=>x.Title).NotEmpty();
        } 
    }
}
