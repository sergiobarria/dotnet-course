using FluentValidation;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Validators;

public class EmployeeForCreationDtoValidator : AbstractValidator<EmployeeForCreationDto>
{
    public EmployeeForCreationDtoValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Employee name is a required field.")
            .MaximumLength(30).WithMessage("Maximum length for the Name is 30 characters.");

        RuleFor(e => e.Age)
            .NotEmpty().WithMessage("Age is a required field.")
            .GreaterThanOrEqualTo(18).WithMessage("Age must be greater than or equal to 18.");

        RuleFor(e => e.Position)
            .NotEmpty().WithMessage("Position is a required field.")
            .MaximumLength(30).WithMessage("Maximum length for the Position is 30 characters.");
    }
}