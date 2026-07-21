using Application.DTOs.StoreDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddInitialStoreInfoDtoValidator:AbstractValidator<AddInitialStoreInfoDto>
    {
        public AddInitialStoreInfoDtoValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.GovernorateId).Must(x => x > 0);
            RuleFor(x => x.SellerId).NotNull();
        }
    }
}
