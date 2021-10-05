using System;
using System.Linq;
using TaxCalculator.DataAccess.Repositories.Interfaces;
using TaxCalculator.Domain;

namespace TaxCalculator.Service
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalculatorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Tax CalculateTax(string postalCode, decimal salary)
        {
            var config = _unitOfWork.TaxTypePostalCodeConfigurationRepository.GetAll(w => w.PostalCode.Equals(postalCode)).FirstOrDefault();
            if (config == null)
            {
                throw new ArgumentException("PostalCode configuration not found");
            }

            Tax tax = new Tax(postalCode, salary, (TaxTypeEnum.TaxType)config.Type);
            return tax.CalculateTax(tax);
        }
    }
}
