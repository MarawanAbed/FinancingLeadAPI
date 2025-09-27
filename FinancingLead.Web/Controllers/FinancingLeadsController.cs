using FinancingLead.Application.Commands.ReviewFinancingLead;
using FinancingLead.Application.Commands.SubmitFinancingLead;
using FinancingLead.Application.Queries.GetFinancingLeadById;
using FinancingLead.Application.Queries.GetFinancingLeads;
using FinancingLead.Domain.Enums;
using FinancingLead.Web.Models.Requests;
using FinancingLead.Web.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancingLead.Web.Controllers
{
    [Route("api/financing-leads")]
    [ApiController]
    public class FinancingLeadsController(IMediator mediator, ILogger<FinancingLeadsController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<FinancingLeadsController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> SubmitLead(
        [FromBody] SubmitFinancingLeadCommand command,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Submitting financing lead for email: {Email}", command.Email);

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Failed",
                    Detail = string.Join("; ", result.Errors ?? new List<string>()),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var response = new SubmitFinancingLeadResponse
            {
                LeadId = result.Data!,
                Message = result.Message ?? "Lead submitted successfully"
            };

            return CreatedAtAction(
                nameof(GetLeadById),
                new { id = result.Data },
                response);
        }
        [HttpGet]
        public async Task<IActionResult> GetLeads(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? phone = null,
        [FromQuery] string? phoneStartsWith = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] LeadReviewStatus? status = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDesc = true,
        CancellationToken cancellationToken = default)
        {
            var query = new GetFinancingLeadsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                Phone = phone,
                PhoneStartsWith = phoneStartsWith,
                From = from,
                To = to,
                Status = status,
                SortBy = sortBy,
                SortDescending = sortDesc
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Query Failed",
                    Detail = string.Join("; ", result.Errors ?? new List<string>()),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var response = new GetFinancingLeadsResponse
            {
                Data = result.Data!,
                Message = result.Message
            };

            return Ok(response);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLeadById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        {
            var query = new GetFinancingLeadByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Lead Not Found",
                    Detail = result.Errors?.FirstOrDefault() ?? "Lead not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var response = new GetFinancingLeadByIdResponse
            {
                Data = result.Data!,
                Message = result.Message
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/review")]
        public async Task<IActionResult> ReviewLead(
        [FromRoute] Guid id,
        [FromBody] ReviewLeadRequest request,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reviewing lead {LeadId} with decision: {Decision}", id, request.Decision);

            var command = new ReviewFinancingLeadCommand
            {
                LeadId = id,
                Decision = request.Decision,
                Reason = request.Reason
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                var statusCode = result.Errors?.FirstOrDefault()?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                    ? StatusCodes.Status404NotFound
                    : StatusCodes.Status400BadRequest;

                return Problem(
                    title: statusCode == 404 ? "Lead Not Found" : "Review Failed",
                    detail: string.Join("; ", result.Errors ?? new List<string>()),
                    statusCode: statusCode);
            }

            var response = new ReviewFinancingLeadResponse
            {
                Success = result.Data!,
                Message = result.Message ?? $"Lead {request.Decision.ToString().ToLower()} successfully"
            };

            return Ok(response);
        }
    }
}
