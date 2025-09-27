using MediatR;
using Microsoft.Extensions.Logging;
using FinancingLead.Application.Common;
using FinancingLead.Application.DTOs;
using FinancingLead.Application.Interfaces;

namespace FinancingLead.Application.Queries.GetFinancingLeadById;

public class GetFinancingLeadByIdQueryHandler(IFinancingLeadRepository repository, ILogger<GetFinancingLeadByIdQueryHandler> logger) : IRequestHandler<GetFinancingLeadByIdQuery, BaseResponse<FinancingLeadDto>>
{
    private readonly IFinancingLeadRepository _repository = repository;
    private readonly ILogger<GetFinancingLeadByIdQueryHandler> _logger = logger;

    public async Task<BaseResponse<FinancingLeadDto>> Handle(GetFinancingLeadByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting financing lead by ID: {LeadId}", request.Id);

            var lead = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (lead == null)
            {
                _logger.LogWarning("Lead not found: {LeadId}", request.Id);
                return BaseResponse<FinancingLeadDto>.ErrorResponse("Lead not found");
            }

            var dto = FinancingLeadDto.FromEntity(lead);

            _logger.LogInformation("Retrieved lead: {LeadId}", request.Id);

            return BaseResponse<FinancingLeadDto>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lead by ID: {LeadId}", request.Id);
            return BaseResponse<FinancingLeadDto>.ErrorResponse("An error occurred while retrieving the lead");
        }
    }
}