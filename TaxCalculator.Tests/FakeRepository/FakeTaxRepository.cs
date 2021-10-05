using System.Collections.Generic;
using TaxCalculator.Domain;

namespace TaxCalculator.Tests.FakeRepository
{
    public class FakeTaxRepository
    {
        private ICollection<Tax> data;


        public FakeTaxRepository(ICollection<Tax> data)
        {
            this.data = data;
        }

        public void Add(Tax tax)
        {
            data.Add(tax);
        }

        public IEnumerable<Tax> GetContacts()
        {
            return data;
        }
    }
}
