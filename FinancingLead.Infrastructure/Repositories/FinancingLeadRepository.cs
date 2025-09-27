

using FinancingLead.Application.Common;
using FinancingLead.Application.DTOs;
using FinancingLead.Application.Interfaces;
using FinancingLead.Domain.Enums;
using FinancingLead.Infrastructure.Dbcontext;
using Financing= FinancingLead.Domain.Entities.FinancingLead;
using Microsoft.EntityFrameworkCore;

namespace FinancingLead.Infrastructure.Repositories;

public class FinancingLeadRepository(ApplicationDbContext context) : IFinancingLeadRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Financing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FinancingLeads
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Financing> AddAsync(Financing lead, CancellationToken cancellationToken = default)
    {
        var entry = await _context.FinancingLeads.AddAsync(lead, cancellationToken);
        return entry.Entity;
    }

    public void Update(Financing lead)
    {
        _context.FinancingLeads.Update(lead);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.FinancingLeads
            .AnyAsync(x => x.Contact.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
    }

    public async Task<bool> ExistsByPhoneAsync(string phoneE164, CancellationToken cancellationToken = default)
    {
        return await _context.FinancingLeads
            .AnyAsync(x => x.Contact.PhoneE164 == phoneE164, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<PaginatedResult<Financing>> GetFinancingLeadsAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = _context.FinancingLeads.AsNoTracking();

        query = ApplyFilters(query, search, phone, phoneStartsWith, from, to, status);

        query = ApplySorting(query, sortBy, sortDescending);

        var totalCount = await query.CountAsync(cancellationToken);

        var leads = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);


        return new PaginatedResult<Financing>(leads, totalCount, pageNumber, pageSize);

    }

    private static IQueryable<Financing> ApplyFilters(
        IQueryable<Financing> query,
        string? search,
        string? phone,
        string? phoneStartsWith,
        DateTime? from,
        DateTime? to,
        LeadReviewStatus? status)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.ToLower();
            query = query.Where(x =>
                x.Contact.Name.ToLower().Contains(searchTerm) ||
                x.Contact.Email.ToLower().Contains(searchTerm) ||
                x.Contact.PhoneE164.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(x => x.Contact.PhoneE164 == phone);
        }

        if (!string.IsNullOrWhiteSpace(phoneStartsWith))
        {
            query = query.Where(x => x.Contact.PhoneE164.StartsWith(phoneStartsWith));
        }

        if (from.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= from.Value);
        }

        if (to.HasValue)
        {
            var toEndOfDay = to.Value.Date.AddDays(1);
            query = query.Where(x => x.CreatedAt < toEndOfDay);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.ReviewStatus == status.Value);
        }

        return query;
    }

    private static IQueryable<Financing> ApplySorting(
        IQueryable<Financing> query,
        string sortBy,
        bool sortDescending)
    {
        return sortBy.ToLower() switch
        {
            "name" => sortDescending
                ? query.OrderByDescending(x => x.Contact.Name)
                : query.OrderBy(x => x.Contact.Name),

            "status" => sortDescending
                ? query.OrderByDescending(x => x.ReviewStatus)
                : query.OrderBy(x => x.ReviewStatus),

            _ => sortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt)
        };
    }
}