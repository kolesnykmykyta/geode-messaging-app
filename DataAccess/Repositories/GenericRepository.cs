using DataAccess.DbContext;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _context;
        private readonly PropertyInfo[] _entityProperties;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _entityProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public void Delete(int id)
        {
            TEntity? entityToDelete = _context.Set<TEntity>().Find(id);
            if (entityToDelete != null)
            {
                _context.Set<TEntity>().Remove(entityToDelete);
            }
        }

        public IEnumerable<TEntity> GetList(string? searchParam = null, string? sortingProp = null, bool sortDescending = false, int? pageSize = null, int? pageNumber = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (sortingProp != null)
            {
                PropertyInfo? entityProp = typeof(TEntity).GetProperty(sortingProp);
                if (entityProp != null)
                {
                    query = sortDescending ?
                        query.OrderByDescending(SortingExpression(entityProp)!) : query.OrderBy(SortingExpression(entityProp)!);
                }
            }

            if (searchParam != null)
            {
                query = query.Where(ObjectContainsString(searchParam));
            }

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize);
            }



            return query.ToList();
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

        private Expression<Func<TEntity, bool>> ObjectContainsString(string searchParam)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            Expression body = Expression.Constant(false);

            foreach (var property in _entityProperties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var propValue = Expression.Property(parameter, property);
                    var searchValue = Expression.Constant(searchParam);
                    var containsCall = Expression.Call(propValue, containsMethod, searchValue);
                    body = Expression.Or(body, containsCall);
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        private Expression<Func<TEntity, object>>? SortingExpression(PropertyInfo sortProperty)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");

            var propertyAccess = Expression.Property(parameter, sortProperty);
            var convert = Expression.Convert(propertyAccess, typeof(object));

            return Expression.Lambda<Func<TEntity, object>>(convert, parameter);
        }
    }
}
