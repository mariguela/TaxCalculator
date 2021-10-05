using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain;

namespace TaxCalculator.DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<Tax> TaxRepository { get; }
        IBaseRepository<TaxTypePostalCodeConfiguration> TaxTypePostalCodeConfigurationRepository { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}
