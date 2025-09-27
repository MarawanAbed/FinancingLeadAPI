using MediatR;
using FinancingLead.Application.Common;
using FinancingLead.Application.DTOs;

namespace FinancingLead.Application.Queries.GetFinancingLeadById;

public class GetFinancingLeadByIdQuery(Guid id) : IRequest<BaseResponse<FinancingLeadDto>>
{
    public Guid Id { get; set; } = id;
}