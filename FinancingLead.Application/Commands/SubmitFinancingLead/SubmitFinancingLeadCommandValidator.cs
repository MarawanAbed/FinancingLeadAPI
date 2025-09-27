using FluentValidation;

namespace FinancingLead.Application.Commands.SubmitFinancingLead;

public class SubmitFinancingLeadCommandValidator : AbstractValidator<SubmitFinancingLeadCommand>
{
    public SubmitFinancingLeadCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid")
            .MaximumLength(254).WithMessage("Email cannot exceed 254 characters");

        RuleFor(x => x.PhoneE164)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+\d{7,15}$").WithMessage("Phone must be in E.164 format (e.g., +966123456789)");

        RuleFor(x => x.TypeOfActivity)
            .NotEmpty().WithMessage("Type of activity is required")
            .MaximumLength(200).WithMessage("Type of activity cannot exceed 200 characters");

        RuleFor(x => x.CommercialRegisterType)
            .IsInEnum().WithMessage("Invalid commercial register type");

        RuleFor(x => x.AnnualIncome)
            .GreaterThanOrEqualTo(0).WithMessage("Annual income cannot be negative")
            .LessThanOrEqualTo(999_999_999.99m).WithMessage("Annual income cannot exceed 999,999,999.99")
            .When(x => x.AnnualIncome.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}