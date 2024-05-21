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
            DeleteWithObjectId(id);
        }

        public void Delete(string id)
        {
            DeleteWithObjectId(id);
        }

        public IQueryable<TEntity> GetList
            (Dictionary<string, string>? searchParams = null,
            string? sortingProp = null,
            bool sortDescending = false,
            int? pageSize = null,
            int? pageNumber = null,
            IEnumerable<string>? selectProps = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (searchParams != null && searchParams.Count() != 0)
            {
                foreach(string key in searchParams.Keys)
                {
                    PropertyInfo? entityProperty = typeof(TEntity).GetProperty(key);
                    if (entityProperty != null)
                    {
                        query = query.Where(SpecificContainsExpression(entityProperty, searchParams[key]));
                    }
                    else
                    {
                        query = query.Where(GlobalContainsExpression(searchParams[key]));
                    }
                }
            }

            if (sortingProp != null)
            {
                PropertyInfo? entityProp = typeof(TEntity).GetProperty(sortingProp);
                if (entityProp != null)
                {
                    query = sortDescending ?
                        query.OrderByDescending(SortingExpression(entityProp)!) : query.OrderBy(SortingExpression(entityProp)!);
                }
            }

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize);
            }

            if (selectProps != null && selectProps.Count() != 0)
            {
                query = query.Select(x => SelectEntityWithDefinedProperties(x, selectProps));
            }

            return query;
        }

        public TEntity? GetById(int id)
        {
            return GetByObjectId(id);
        }

        public TEntity? GetById(string id)
        {
            return GetByObjectId(id);
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(int id, TEntity entity)
        {
            UpdateWithObjectId(id, entity);
        }

        public void Update(string id, TEntity entity)
        {
            UpdateWithObjectId(id, entity);
        }

        private Expression<Func<TEntity, bool>> SpecificContainsExpression(PropertyInfo property, string searchParam)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var propValue = Expression.Property(parameter, property);
            var searchValue = Expression.Constant(searchParam);
            var containsCall = Expression.Call(propValue, containsMethod, searchValue);

            return Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
        }

        private Expression<Func<TEntity, bool>> GlobalContainsExpression(string searchParam)
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

        private static TEntity SelectEntityWithDefinedProperties(TEntity original, IEnumerable<string> properties)
        {
            TEntity resultEntity = Activator.CreateInstance<TEntity>();

            foreach (string property in properties)
            {
                PropertyInfo? entityProperty = typeof(TEntity).GetProperty(property);

                if (entityProperty != null)
                {
                    entityProperty.SetValue(resultEntity, entityProperty.GetValue(original));
                }
            }

            return resultEntity;
        }

        private TEntity? GetByObjectId(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        private void UpdateWithObjectId(object id, TEntity entity)
        {
            TEntity? entityToUpdate = _context.Set<TEntity>().Find(id);
            if (entity != null && entityToUpdate != null)
            {
                PropertyInfo[] properties = typeof(TEntity).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var newValue = property.GetValue(entity);
                    if (newValue != null)
                    {
                        property.SetValue(entityToUpdate, newValue);
                    }
                }

                _context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }

        private void DeleteWithObjectId(object id)
        {
            TEntity? entityToDelete = _context.Set<TEntity>().Find(id);
            if (entityToDelete != null)
            {
                _context.Set<TEntity>().Remove(entityToDelete);
            }
        }
    }
}
