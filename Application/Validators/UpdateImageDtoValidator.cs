using Application.DTOs.ProductDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UpdateImageDtoValidator : AbstractValidator<UpdateImageDto>
    {
        public UpdateImageDtoValidator()
        {
            RuleFor(x => x.NewImageLink).NotEmpty();
        }
    }
}
