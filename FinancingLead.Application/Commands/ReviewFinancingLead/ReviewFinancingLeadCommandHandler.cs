using MediatR;
using Microsoft.Extensions.Logging;
using FinancingLead.Application.Common;
using FinancingLead.Application.Interfaces;
using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.Commands.ReviewFinancingLead;

public class ReviewFinancingLeadCommandHandler(
    IFinancingLeadRepository repository,
    INotificationClient notificationClient,
    ILogger<ReviewFinancingLeadCommandHandler> logger) : IRequestHandler<ReviewFinancingLeadCommand, BaseResponse<bool>>
{
    private readonly IFinancingLeadRepository _repository = repository;
    private readonly INotificationClient _notificationClient = notificationClient;
    private readonly ILogger<ReviewFinancingLeadCommandHandler> _logger = logger;

    public async Task<BaseResponse<bool>> Handle(ReviewFinancingLeadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Reviewing lead {LeadId} with decision: {Decision}", request.LeadId, request.Decision);

            var lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken);
            if (lead == null)
            {
                _logger.LogWarning("Lead not found: {LeadId}", request.LeadId);
                return BaseResponse<bool>.ErrorResponse("Lead not found");
            }

            if (lead.IsInStatus(request.Decision))
            {
                _logger.LogInformation("Lead {LeadId} is already in status: {Status}", request.LeadId, request.Decision);
                return BaseResponse<bool>.SuccessResponse(true, $"Lead is already {request.Decision}");
            }

            var previousStatus = lead.ReviewStatus;

            lead.Review(request.Decision, request.Reason);


            _repository.Update(lead);
            await _repository.SaveChangesAsync(cancellationToken);

            if (request.Decision == LeadReviewStatus.Accepted && previousStatus != LeadReviewStatus.Accepted)
            {
                await SendAcceptanceNotification(lead, cancellationToken);
            }

            _logger.LogInformation("Successfully reviewed lead {LeadId} with decision: {Decision}", request.LeadId, request.Decision);

            return BaseResponse<bool>.SuccessResponse(true, $"Lead {request.Decision.ToString().ToLower()} successfully");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business rule violation while reviewing lead {LeadId}", request.LeadId);
            return BaseResponse<bool>.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reviewing lead {LeadId}", request.LeadId);
            return BaseResponse<bool>.ErrorResponse("An error occurred while reviewing the lead");
        }
    }

    private async Task SendAcceptanceNotification(Domain.Entities.FinancingLead lead, CancellationToken cancellationToken)
    {
        try
        {
            await _notificationClient.SendNotificationAsync(
                lead.Contact.PhoneE164,
                "Lead Accepted",
                "Your financing lead was accepted.",
                cancellationToken);

            _logger.LogInformation("Notification sent for accepted lead: {LeadId}", lead.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification for lead: {LeadId}", lead.Id);
         
        }
    }
}