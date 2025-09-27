

using FinancingLead.Application.Common;
using FinancingLead.Domain.Enums;
using MediatR;

namespace FinancingLead.Application.Commands.SubmitFinancingLead;

public class SubmitFinancingLeadCommand : IRequest<BaseResponse<Guid>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneE164 { get; set; } = null!;
    public PreferredContactMethod? PreferredContactMethod { get; set; }
    public string TypeOfActivity { get; set; } = null!;
    public CommercialRegisterType CommercialRegisterType { get; set; }
    public decimal? AnnualIncome { get; set; }
    public string? Notes { get; set; }
}
