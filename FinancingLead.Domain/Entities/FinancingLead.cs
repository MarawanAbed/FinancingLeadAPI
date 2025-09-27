using FinancingLead.Domain.Common;
using FinancingLead.Domain.Enums;
using FinancingLead.Domain.Events;
using FinancingLead.Domain.ValueObjects;

namespace FinancingLead.Domain.Entities;

public class FinancingLead : BaseEntity
{
    public ContactInformation Contact { get; private set; }
    public BusinessInformation Business { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public LeadReviewStatus ReviewStatus { get; private set; }
    public string? ReviewReason { get; private set; }

    private FinancingLead() { }

    private FinancingLead(
        ContactInformation contact,
        BusinessInformation business,
        string? notes = null)
    {
        Contact = contact;
        Business = business;
        Notes = ValidateAndNormalizeNotes(notes);
        ReviewStatus = LeadReviewStatus.Pending;
        ReviewedAt = null;
        ReviewReason = null;
    }

    public static FinancingLead Create(
        string name,
        string email,
        string phoneE164,
        string typeOfActivity,
        CommercialRegisterType commercialRegisterType,
        PreferredContactMethod? preferredContactMethod = null,
        decimal? annualIncome = null,
        string? notes = null)
    {
        var contact = ContactInformation.Create(name, email, phoneE164, preferredContactMethod);
        var business = BusinessInformation.Create(typeOfActivity, commercialRegisterType, annualIncome);

        return new FinancingLead(contact, business, notes);
    }

    public void Review(LeadReviewStatus newStatus, string? reason = null)
    {
        if (newStatus == LeadReviewStatus.Pending)
            throw new InvalidOperationException("Cannot review a lead back to Pending status");

        var previousStatus = ReviewStatus;

        ReviewStatus = newStatus;
        ReviewedAt = DateTime.UtcNow;
        ReviewReason = reason?.Trim();

        if (newStatus == LeadReviewStatus.Accepted && previousStatus != LeadReviewStatus.Accepted)
        {
            AddDomainEvent(new LeadAcceptedDomainEvent(Id, Contact.PhoneE164));
        }
    }

    private static string? ValidateAndNormalizeNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
            return null;

        var trimmed = notes.Trim();

        if (trimmed.Length > 1000)
            throw new ArgumentException("Notes cannot exceed 1000 characters", nameof(notes));

        return trimmed;
    }

    public bool IsInStatus(LeadReviewStatus status)
    {
        return ReviewStatus == status;
    }
}