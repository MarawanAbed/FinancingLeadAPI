using MediatR;
using Microsoft.Extensions.Logging;
using FinancingLead.Application.Common;
using FinancingLead.Application.Interfaces;

namespace FinancingLead.Application.Commands.SubmitFinancingLead;

public class SubmitFinancingLeadCommandHandler(
    IFinancingLeadRepository repository,
    ILogger<SubmitFinancingLeadCommandHandler> logger) : IRequestHandler<SubmitFinancingLeadCommand, BaseResponse<Guid>>
{
    private readonly IFinancingLeadRepository _repository = repository;
    private readonly ILogger<SubmitFinancingLeadCommandHandler> _logger = logger;

    public async Task<BaseResponse<Guid>> Handle(SubmitFinancingLeadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Submitting financing lead for email: {Email}", request.Email);

            var existingEmail = await _repository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (existingEmail)
            {
                _logger.LogWarning("Duplicate email attempted: {Email}", request.Email);
                return BaseResponse<Guid>.ErrorResponse("A lead with this email already exists");
            }

            var existingPhone = await _repository.ExistsByPhoneAsync(request.PhoneE164, cancellationToken);
            if (existingPhone)
            {
                _logger.LogWarning("Duplicate phone attempted: {Phone}", request.PhoneE164);
                return BaseResponse<Guid>.ErrorResponse("A lead with this phone number already exists");
            }

            var lead = Domain.Entities.FinancingLead.Create(
                name: request.Name,
                email: request.Email,
                phoneE164: request.PhoneE164,
                typeOfActivity: request.TypeOfActivity,
                commercialRegisterType: request.CommercialRegisterType,
                preferredContactMethod: request.PreferredContactMethod,
                annualIncome: request.AnnualIncome,
                notes: request.Notes
            );

            await _repository.AddAsync(lead, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created financing lead with ID: {LeadId}", lead.Id);

            return BaseResponse<Guid>.SuccessResponse(lead.Id, "Financing lead submitted successfully");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while submitting lead");
            return BaseResponse<Guid>.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting financing lead");
            return BaseResponse<Guid>.ErrorResponse("An error occurred while submitting the lead");
        }
    }
}