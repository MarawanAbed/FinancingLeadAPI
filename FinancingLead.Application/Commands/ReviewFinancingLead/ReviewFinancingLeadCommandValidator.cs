using FluentValidation;
using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.Commands.ReviewFinancingLead;

public class ReviewFinancingLeadCommandValidator : AbstractValidator<ReviewFinancingLeadCommand>
{
    public ReviewFinancingLeadCommandValidator()
    {
        RuleFor(x => x.LeadId)
            .NotEmpty().WithMessage("Lead ID is required");

        RuleFor(x => x.Decision)
            .NotEqual(LeadReviewStatus.Pending).WithMessage("Cannot review a lead back to Pending status")
            .IsInEnum().WithMessage("Invalid review decision");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Review reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}