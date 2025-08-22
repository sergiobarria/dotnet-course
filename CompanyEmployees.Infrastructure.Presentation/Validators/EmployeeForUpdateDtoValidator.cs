using FluentValidation;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Validators;

public class EmployeeForUpdateDtoValidator : AbstractValidator<EmployeeForUpdateDto>
{
    public EmployeeForUpdateDtoValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Employee name is a required field.")
            .MaximumLength(30).WithMessage("\"Maximum length for the Name is 30 characters.");

        RuleFor(e => e.Age)
            .NotEmpty().WithMessage("Age is a required field.")
            .GreaterThanOrEqualTo(18).WithMessage("Age can't be lower than 18.");

        RuleFor(e => e.Position)
            .NotEmpty().WithMessage("Position is a required field.")
            .MaximumLength(20).WithMessage("Maximum length for the Position is 20 characters.");
    }
}