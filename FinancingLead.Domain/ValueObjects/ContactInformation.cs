using FinancingLead.Domain.Enums;
using System.Text.RegularExpressions;

namespace FinancingLead.Domain.ValueObjects;

public class ContactInformation
{
    public string Name { get; }
    public string Email { get; }
    public string PhoneE164 { get; }
    public PreferredContactMethod? PreferredContactMethod { get; }

    private ContactInformation(string name, string email, string phoneE164, PreferredContactMethod? preferredContactMethod)
    {
        Name = name;
        Email = email;
        PhoneE164 = phoneE164;
        PreferredContactMethod = preferredContactMethod;
    }

    public static ContactInformation Create(string name, string email, string phoneE164, PreferredContactMethod? preferredContactMethod = null)
    {
        var normalizedName = NormalizeName(name);
        var normalizedEmail = NormalizeEmail(email);
        var normalizedPhone = NormalizePhoneE164(phoneE164);

        ValidateInputs(normalizedName, normalizedEmail, normalizedPhone);

        return new ContactInformation(normalizedName, normalizedEmail, normalizedPhone, preferredContactMethod);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        return name.Trim();
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        return email.Trim().ToLowerInvariant();
    }

    private static string NormalizePhoneE164(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty", nameof(phone));

        var normalized = Regex.Replace(phone.Trim(), @"[^\d+]", "");

        if (!normalized.StartsWith("+"))
            normalized = "+" + normalized;

        return normalized;
    }

    private static void ValidateInputs(string name, string email, string phone)
    {
        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        if (!IsValidPhoneE164(phone))
            throw new ArgumentException("Invalid phone E.164 format", nameof(phone));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneE164(string phone)
    {
        return Regex.IsMatch(phone, @"^\+\d{7,15}$");
    }
}
