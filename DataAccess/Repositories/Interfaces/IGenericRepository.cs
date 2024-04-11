using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        public IQueryable<TEntity> GetList
            (Dictionary<string, string>? searchParams = null,
            string? sortingProp = null,
            bool sortDescending = false,
            int? pageSize = null,
            int? pageNumber = null,
            IEnumerable<string>? selectProps = null);

        public TEntity? GetById(int id);
        public void Insert(TEntity entity);
        public void Update(int id, TEntity entity);
        public void Delete(int id);
    }
}
