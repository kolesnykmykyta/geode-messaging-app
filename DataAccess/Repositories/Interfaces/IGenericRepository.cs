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
        public IEnumerable<TEntity> GetList(IEnumerable<Expression<Func<TEntity, bool>>>? conditions, int? pageSize, int? pageNumber);
        public TEntity? GetById(int id);
        public void Insert(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
    }
}
