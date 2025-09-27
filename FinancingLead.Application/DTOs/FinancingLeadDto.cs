

using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.DTOs;

public class FinancingLeadDto
{
    public Guid Id { get; set; }
    public ContactInformationDto Contact { get; set; } = null!;
    public BusinessInformationDto Business { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public LeadReviewStatus ReviewStatus { get; set; }
    public string? ReviewReason { get; set; }

    public static FinancingLeadDto FromEntity(Domain.Entities.FinancingLead lead)
    {
        return new FinancingLeadDto
        {
            Id = lead.Id,
            Contact = ContactInformationDto.FromValueObject(lead.Contact),
            Business = BusinessInformationDto.FromValueObject(lead.Business),
            Notes = lead.Notes,
            CreatedAt = lead.CreatedAt,
            ReviewedAt = lead.ReviewedAt,
            ReviewStatus = lead.ReviewStatus,
            ReviewReason = lead.ReviewReason
        };
    }
}



