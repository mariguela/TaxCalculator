using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.DataAccess.Repositories.Interfaces;
using TaxCalculator.Domain;

namespace TaxCalculator.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EntitiesContext _contexto = null;
        private IDbContextTransaction _objTran;
        private IBaseRepository<Tax> taxRepository = null;
        private IBaseRepository<TaxTypePostalCodeConfiguration> taxTypePostalCodeConfigurationRepositoryRepository = null; 

        public UnitOfWork(EntitiesContext context)
        {
            this._contexto = context;
        }

        public IBaseRepository<Tax> TaxRepository
        {
            get
            {
                if (taxRepository == null)
                {
                    taxRepository = new BaseRepository<Tax>(_contexto);
                }
                return taxRepository;
            }
        }

        public IBaseRepository<TaxTypePostalCodeConfiguration> TaxTypePostalCodeConfigurationRepository
        {
            get
            {
                if (taxTypePostalCodeConfigurationRepositoryRepository == null)
                {
                    taxTypePostalCodeConfigurationRepositoryRepository = new BaseRepository<TaxTypePostalCodeConfiguration>(_contexto);
                }
                return taxTypePostalCodeConfigurationRepositoryRepository;
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contexto.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateTransaction()
        {
            _objTran = _contexto.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public void Save()
        {
            _contexto.SaveChanges();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
