using FluentValidation;

namespace FinancingLead.Application.Queries.GetFinancingLeads;

public class GetFinancingLeadsQueryValidator : AbstractValidator<GetFinancingLeadsQuery>
{
    private static readonly string[] AllowedSortFields = { "CreatedAt", "Name", "Status" };

    public GetFinancingLeadsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.From)
            .LessThanOrEqualTo(x => x.To).WithMessage("From date must be less than or equal to To date")
            .When(x => x.From.HasValue && x.To.HasValue);

        RuleFor(x => x.SortBy)
            .Must(x => string.IsNullOrEmpty(x) || AllowedSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortFields)}");
    }
}