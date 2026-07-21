using Application.DTOs.PersonDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddInitialPersonInfoDtoValidator : AbstractValidator<AddInitialPersonInfoDto>
    {
        public AddInitialPersonInfoDtoValidator()
        {
            RuleFor(x => x.FristName).NotEmpty().WithMessage("الأسم الأول مطلوب");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("الأسم الأخير مطلوب");
            RuleFor(x => x.GovernorateId).NotNull().Must(x => x > 0).WithMessage("رقم المحافطة خاطئ");
        }
    }
}
