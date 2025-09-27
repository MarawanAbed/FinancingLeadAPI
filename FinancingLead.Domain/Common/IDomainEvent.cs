

namespace FinancingLead.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
