using FinancingLead.Domain.Enums;

namespace FinancingLead.Application.DTOs;

public class BusinessInformationDto
{
    public string TypeOfActivity { get; set; } = null!;
    public CommercialRegisterType CommercialRegisterType { get; set; }
    public decimal? AnnualIncome { get; set; }

    public static BusinessInformationDto FromValueObject(Domain.ValueObjects.BusinessInformation business)
    {
        return new BusinessInformationDto
        {
            TypeOfActivity = business.TypeOfActivity,
            CommercialRegisterType = business.CommercialRegisterType,
            AnnualIncome = business.AnnualIncome
        };
    }
}
