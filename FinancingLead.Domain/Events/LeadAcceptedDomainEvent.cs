

using FinancingLead.Domain.Common;

namespace FinancingLead.Domain.Events;

public class LeadAcceptedDomainEvent(Guid leadId, string phoneE164) : IDomainEvent
{
    public Guid LeadId { get; } = leadId;
    public string PhoneE164 { get; } = phoneE164;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
