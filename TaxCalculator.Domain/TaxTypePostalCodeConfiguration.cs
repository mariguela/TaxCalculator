using System;
using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Domain
{
    public class TaxTypePostalCodeConfiguration
    {
        [Key]
        public string PostalCode { get; set; }

        public Int16 Type { get; set; }
    }
}