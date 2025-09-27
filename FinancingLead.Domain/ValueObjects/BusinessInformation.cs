

using FinancingLead.Domain.Enums;

namespace FinancingLead.Domain.ValueObjects;

public class BusinessInformation
{
    public string TypeOfActivity { get; }
    public CommercialRegisterType CommercialRegisterType { get; }
    public decimal? AnnualIncome { get; }

    private BusinessInformation(string typeOfActivity, CommercialRegisterType commercialRegisterType, decimal? annualIncome)
    {
        TypeOfActivity = typeOfActivity;
        CommercialRegisterType = commercialRegisterType;
        AnnualIncome = annualIncome;
    }

    public static BusinessInformation Create(string typeOfActivity, CommercialRegisterType commercialRegisterType, decimal? annualIncome = null)
    {
        var normalizedActivity = NormalizeTypeOfActivity(typeOfActivity);
        ValidateInputs(normalizedActivity, annualIncome);

        return new BusinessInformation(normalizedActivity, commercialRegisterType, annualIncome);
    }

    private static string NormalizeTypeOfActivity(string typeOfActivity)
    {
        if (string.IsNullOrWhiteSpace(typeOfActivity))
            throw new ArgumentException("Type of activity cannot be empty", nameof(typeOfActivity));

        return typeOfActivity.Trim();
    }

    private static void ValidateInputs(string typeOfActivity, decimal? annualIncome)
    {
        if (typeOfActivity.Length > 200)
            throw new ArgumentException("Type of activity cannot exceed 200 characters", nameof(typeOfActivity));

        if (annualIncome.HasValue && annualIncome.Value < 0)
            throw new ArgumentException("Annual income cannot be negative", nameof(annualIncome));

        if (annualIncome.HasValue && annualIncome.Value > 999_999_999.99m)
            throw new ArgumentException("Annual income cannot exceed 999,999,999.99", nameof(annualIncome));
    }

}
