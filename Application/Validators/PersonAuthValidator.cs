using Application.DTOs.AuthDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class PersonAuthValidator : AbstractValidator<PersonAuthDto>
    {
        private static readonly HashSet<string> ValidPhoneCodes = new() { "079", "078", "077" };

        public PersonAuthValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب")
                .Length(10).WithMessage("رقم الهاتف يجب ان يكون مؤلف من 10 ارقام")
                .Must(BeValidPhoneCode).WithMessage(" 079, 078, or 077 يجب ان يبدأ رقم الهاتف");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة السر مطلوبة");
        }

        private static bool BeValidPhoneCode(string phone)
        {
            return !string.IsNullOrEmpty(phone) && phone.Length >= 3 &&
                   ValidPhoneCodes.Contains(phone[..3]);
        }
    }
}
