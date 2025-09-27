

using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FinancingLead.Application.Interfaces;

namespace FinancingLead.Infrastructure.Services;

public class FirebaseNotificationClient : INotificationClient
{
    private readonly ILogger<FirebaseNotificationClient> _logger;
    private readonly string _projectId;

    public FirebaseNotificationClient(IConfiguration configuration, ILogger<FirebaseNotificationClient> logger)
    {
        _logger = logger;
        _projectId = configuration["Firebase:ProjectId"] ?? throw new ArgumentException("Firebase:ProjectId is required");

        
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                ProjectId = _projectId
            });
        }
    }

    public async Task SendNotificationAsync(string phoneE164, string title, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var topicName = NormalizePhoneToTopic(phoneE164);

            var message = new Message()
            {
                Topic = topicName,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    {"phone", phoneE164},
                    {"timestamp", DateTime.UtcNow.ToString("O")}
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);

            _logger.LogInformation("Successfully sent notification to topic {Topic} for phone {Phone}. MessageId: {MessageId}",
                topicName, phoneE164, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification to phone {Phone}", phoneE164);
            throw;
        }
    }

    public async Task SendNotificationToTopicAsync(string topic, string title, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    {"timestamp", DateTime.UtcNow.ToString("O")}
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);

            _logger.LogInformation("Successfully sent notification to topic {Topic}. MessageId: {MessageId}",
                topic, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification to topic {Topic}", topic);
            throw;
        }
    }

    private static string NormalizePhoneToTopic(string phoneE164)
    {
        return phoneE164
            .Replace("+", "plus")
            .Replace("-", "")
            .Replace(" ", "")
            .Replace("(", "")
            .Replace(")", "");
    }
}
