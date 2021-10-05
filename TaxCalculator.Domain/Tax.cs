using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaxCalculator.Framework;
using static TaxCalculator.Domain.TaxTypeEnum;

namespace TaxCalculator.Domain
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        public string PostalCode { get; set; }

        public decimal Salary { get; set; }

        public DateTime CreateAt { get; private set; }

        public TaxType Type { get; private set; }

        public decimal TaxValue { get; private set; }

        [NotMapped]
        public decimal SalaryWithTax 
        {
            get => TaxValue + Salary;  
        }

        public Tax()
        {

        }

        public Tax(string postalCode, decimal salary, TaxType taxType)
        {
            this.Salary = salary;
            this.PostalCode = postalCode;
            this.Type = taxType;
            this.TaxValue = CalculateTax(this).TaxValue;
        }

        public Tax CalculateTax(Tax tax)
        {
            if (tax.Type == TaxTypeEnum.TaxType.FlatRate)
            {
                tax.TaxValue = tax.Salary * 0.175m;
            }
            else if (tax.Type == TaxTypeEnum.TaxType.FlatValue)
            {
                if (tax.Salary <= 200000)
                {
                    tax.TaxValue = tax.Salary * 0.05m;
                }
                else
                {
                    tax.TaxValue = 10000;
                }
            }
            else if (tax.Type == TaxTypeEnum.TaxType.Progressive)
            {
                var taxBands = new[]
                {
                    new { Min = 0m, Max = 8350m, Rate = 0.10m },
                    new { Min = 8351m, Max = 33950m, Rate = 0.15m },
                    new { Min = 33951m, Max = 82250m, Rate = 0.25m },
                    new { Min = 82251m, Max = 171550m, Rate = 0.28m },
                    new { Min = 171551m, Max = 372950m, Rate = 0.33m },
                    new { Min = 372951m, Max = decimal.MaxValue, Rate = 0.35m }
                };

                var calculateProgressiveTax = 0m;
                foreach (var band in taxBands)
                {
                    if (tax.Salary > band.Min)
                    {
                        var taxRate = Math.Min(band.Max - band.Min, tax.Salary - band.Min);
                        var taxBand = taxRate * band.Rate;
                        calculateProgressiveTax += taxBand;
                    }
                }
                tax.TaxValue = calculateProgressiveTax;
            }
            return tax;
        }

    }
}
