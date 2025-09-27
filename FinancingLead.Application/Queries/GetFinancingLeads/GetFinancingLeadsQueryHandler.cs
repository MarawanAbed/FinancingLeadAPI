using MediatR;
using Microsoft.Extensions.Logging;
using FinancingLead.Application.Common;
using FinancingLead.Application.DTOs;
using FinancingLead.Application.Interfaces;

namespace FinancingLead.Application.Queries.GetFinancingLeads;

public class GetFinancingLeadsQueryHandler(IFinancingLeadRepository repository, ILogger<GetFinancingLeadsQueryHandler> logger) : IRequestHandler<GetFinancingLeadsQuery, BaseResponse<PaginatedResult<FinancingLeadDto>>>
{
    private readonly IFinancingLeadRepository _repository = repository;
    private readonly ILogger<GetFinancingLeadsQueryHandler> _logger = logger;

    public async Task<BaseResponse<PaginatedResult<FinancingLeadDto>>> Handle(GetFinancingLeadsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting financing leads - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

            var result = await _repository.GetFinancingLeadsAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                search: request.Search,
                phone: request.Phone,
                phoneStartsWith: request.PhoneStartsWith,
                from: request.From,
                to: request.To,
                status: request.Status,
                sortBy: request.SortBy ?? "CreatedAt",
                sortDescending: request.SortDescending,
                cancellationToken: cancellationToken);

            _logger.LogInformation("Retrieved {Count} leads out of {TotalCount}", result.Items.Count, result.TotalCount);
            var dtoItems = result.Items.Select(FinancingLeadDto.FromEntity).ToList();

            var paginatedDto = new PaginatedResult<FinancingLeadDto>(
                items: dtoItems,
                totalCount: result.TotalCount,
                pageNumber: result.PageNumber,
                pageSize: result.PageSize
            );

            return BaseResponse<PaginatedResult<FinancingLeadDto>>.SuccessResponse(paginatedDto);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting financing leads");
            return BaseResponse<PaginatedResult<FinancingLeadDto>>.ErrorResponse("An error occurred while retrieving leads");
        }
    }
}