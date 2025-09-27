

using FinancingLead.Application.Common;
using FinancingLead.Domain.Enums;
using FinancingLead.Application.DTOs;
using Financing = FinancingLead.Domain.Entities.FinancingLead;

namespace FinancingLead.Application.Interfaces;

public interface IFinancingLeadRepository
{
    Task<Financing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Financing> AddAsync(Financing lead, CancellationToken cancellationToken = default);
    void Update(Financing lead);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPhoneAsync(string phoneE164, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<PaginatedResult<Financing>> GetFinancingLeadsAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        string? phone = null,
        string? phoneStartsWith = null,
        DateTime? from = null,
        DateTime? to = null,
        LeadReviewStatus? status = null,
        string sortBy = "CreatedAt",
        bool sortDescending = true,
        CancellationToken cancellationToken = default);
}