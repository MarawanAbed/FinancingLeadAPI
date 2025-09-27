using FinancingLead.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinancingLead.Web.Models.Requests;

public class ReviewLeadRequest
{
    [Required]
    public LeadReviewStatus Decision { get; set; }

    public string? Reason { get; set; }
}