using Application.DTOs.ProductDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class GetProductsPaginatedRequestDtoValidator : AbstractValidator<GetProductsPaginatedRequestDto>
    {
        public GetProductsPaginatedRequestDtoValidator()
        {
            RuleFor(x => x.Limit).LessThanOrEqualTo(8);
        }
    }
}
