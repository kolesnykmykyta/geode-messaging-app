using DataAccess.DbContext;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _context;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            TEntity? entityToDelete = _context.Set<TEntity>().Find(id);
            if (entityToDelete != null) {
                _context.Set<TEntity>().Remove(entityToDelete);
            }
        }

        public IEnumerable<TEntity> GetList(IEnumerable<Expression<Func<TEntity, bool>>>? conditions = null, int? pageSize = null, int? pageNumber = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (conditions != null)
            {
                foreach(var condition in conditions)
                {
                    query = query.Where(condition);
                }
            }

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize);
            }

            return query;
        }

        public TEntity? GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
