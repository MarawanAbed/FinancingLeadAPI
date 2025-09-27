using FinancingLead.Application.DTOs;

namespace FinancingLead.Web.Models.Response;

public class GetFinancingLeadByIdResponse
{
    public FinancingLeadDto Data { get; set; } = null!;
    public string? Message { get; set; }
}
