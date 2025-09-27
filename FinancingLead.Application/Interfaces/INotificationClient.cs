

namespace FinancingLead.Application.Interfaces;

public interface INotificationClient
{
    Task SendNotificationAsync(string phoneE164, string title, string body, CancellationToken cancellationToken = default);
    Task SendNotificationToTopicAsync(string topic, string title, string body, CancellationToken cancellationToken = default);
}
