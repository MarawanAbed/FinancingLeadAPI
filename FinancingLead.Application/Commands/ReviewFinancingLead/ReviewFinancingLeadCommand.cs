using MediatR;
using FinancingLead.Application.Common;
using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.Commands.ReviewFinancingLead;

public class ReviewFinancingLeadCommand : IRequest<BaseResponse<bool>>
{
    public Guid LeadId { get; set; }
    public LeadReviewStatus Decision { get; set; }
    public string? Reason { get; set; }
}