using MediatR;
using FinancingLead.Application.Common;
using FinancingLead.Application.DTOs;
using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.Queries.GetFinancingLeads;

public class GetFinancingLeadsQuery : IRequest<BaseResponse<PaginatedResult<FinancingLeadDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public string? Phone { get; set; }
    public string? PhoneStartsWith { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public LeadReviewStatus? Status { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}