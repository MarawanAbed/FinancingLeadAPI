namespace FinancingLead.Web.Models.Response;

public class SubmitFinancingLeadResponse
{
    public Guid LeadId { get; set; }
    public string Message { get; set; } = null!;
}