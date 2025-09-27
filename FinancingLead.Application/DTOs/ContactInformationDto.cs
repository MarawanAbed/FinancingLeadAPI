

using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.DTOs;

public class ContactInformationDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneE164 { get; set; } = null!;
    public PreferredContactMethod? PreferredContactMethod { get; set; }

    public static ContactInformationDto FromValueObject(Domain.ValueObjects.ContactInformation contact)
    {
        return new ContactInformationDto
        {
            Name = contact.Name,
            Email = contact.Email,
            PhoneE164 = contact.PhoneE164,
            PreferredContactMethod = contact.PreferredContactMethod
        };
    }
}
