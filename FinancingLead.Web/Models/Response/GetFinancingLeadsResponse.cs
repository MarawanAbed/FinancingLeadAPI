    using FinancingLead.Application.Common;
    using FinancingLead.Application.DTOs;

    namespace FinancingLead.Web.Models.Response;

    public class GetFinancingLeadsResponse
    {
        public PaginatedResult<FinancingLeadDto> Data { get; set; } = null!;
        public string? Message { get; set; }
    }