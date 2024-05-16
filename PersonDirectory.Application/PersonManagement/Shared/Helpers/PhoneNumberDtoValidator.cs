using FluentValidation;
using PersonDirectory.Application.PersonManagement.DTOs;

namespace PersonDirectory.Application.PersonManagement.Shared.Helpers;

public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDTO>
{
    public PhoneNumberDtoValidator()
    {
        RuleFor(x => x.Number).NotEmpty().Length(4, 50);
        RuleFor(x => x.Type).IsInEnum();
    }
}
