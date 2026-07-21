using Application.DTOs.ProductDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddProductDtoValidator : AbstractValidator<AddProductDto>
    {
        public AddProductDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PrimaryImageLink).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Stock).GreaterThan(0);
            RuleFor(x => x.ImagesLinks).NotEmpty();
            RuleFor(x => x.ImagesLinks).Must(x => x.Count <= 3).WithMessage("ImagesLinks must be at most 3 links");
        }
    }
}
