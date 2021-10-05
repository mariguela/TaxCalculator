using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TaxCalculator.DataAccess.Repositories.Interfaces;

namespace TaxCalculator.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly EntitiesContext _ctx;

        public BaseRepository(EntitiesContext contexto)
        {
            this._ctx = contexto;
        }

        public void Add(T entity)
        {
            Type type = entity.GetType();

            PropertyInfo propDataCadastro = type.GetProperty("CreateAt");
            if (propDataCadastro != null)
            {
                propDataCadastro.SetValue(entity, DateTime.Now, null);
            }

            _ctx.Set<T>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Added;
        }


        public void Delete(T entity)
        {
            _ctx.Set<T>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Deleted;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _ctx.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
                query = include(query);


            if (orderBy != null)
            {
                return orderBy(query.AsNoTracking()).ToList();
            }
            else
            {
                return query.AsNoTracking().ToList();
            }

        }

        public void Update(T entity)
        {
            _ctx.Set<T>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}
