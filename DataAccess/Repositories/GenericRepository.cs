using DataAccess.DbContext;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public void Delete(int id)
        {
            TEntity? entityToDelete = _dbset.Find(id);
            if (entityToDelete != null) { 
                _dbset.Remove(entityToDelete);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbset.ToList();
        }

        public TEntity? GetById(int id)
        {
            return _dbset.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbset.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
