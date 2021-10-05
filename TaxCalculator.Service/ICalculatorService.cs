using TaxCalculator.Domain;
using static TaxCalculator.Domain.TaxTypeEnum;

namespace TaxCalculator.Service
{
    public interface ICalculatorService
    {
        Tax CalculateTax(string postalCode, decimal salary);
    }
}
