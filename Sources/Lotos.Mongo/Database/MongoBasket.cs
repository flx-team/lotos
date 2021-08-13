using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using MongoDB.Driver;

namespace Lotos.Mongo.Database
{
    internal sealed class MongoBasket<T> : IBasket<T> where T : IEntity<T>
    {
        private readonly IMongoCollection<T> _collection;
        private readonly FilterDefinitionBuilder<T> _filters;

        public MongoBasket(IMongoCollection<T> collection)
        {
            _collection = collection;
            _filters = Builders<T>.Filter;
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            long count = await _collection.CountDocumentsAsync(expression);

            return (int)count;
        }

        public async Task<bool> Exists(Guid id)
        {
            var idFilter = BuildIdFilter(id);

            var options = new CountOptions { Limit = 1 };

            long count = await _collection.CountDocumentsAsync(idFilter, options);

            return count != 0;
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            var options = new CountOptions { Limit = 1 };

            long count = await _collection.CountDocumentsAsync(expression, options);

            return count != 0;
        }

        public async Task<T> Keep(T entity)
        {
            if (entity.Id != Guid.Empty)
            {
                return entity;
            }

            var id = Guid.NewGuid();

            entity.SetId(id);

            await _collection.InsertOneAsync(entity);

            entity.SetBasket(this);

            return entity;
        }

        public async Task<T?> Pick(Guid id)
        {
            var idFilter = BuildIdFilter(id);

            var result = await _collection.Find(idFilter).FirstOrDefaultAsync();

            result?.SetBasket(this);

            return result;
        }

        public async Task<T?> Pick(Expression<Func<T, bool>> expression)
        {
            var result = await _collection.Find(expression).FirstOrDefaultAsync();

            result?.SetBasket(this);

            return result;
        }

        public Task<IEnumerable<T>> PickMany(params Guid[] ids)
        {
            return PickMany(ids as IEnumerable<Guid>);
        }

        public async Task<IEnumerable<T>> PickMany(IEnumerable<Guid> ids)
        {
            var idsFilter = BuildOrIdsFilter(ids);

            var result = await _collection.Find(idsFilter).ToListAsync();

            foreach (var item in result)
            {
                item?.SetBasket(this);
            }

            return result;
        }

        public async Task<IEnumerable<T>> PickMany(Expression<Func<T, bool>> expression)
        {
            var result = await _collection.Find(expression).ToListAsync();

            foreach (var item in result)
            {
                item?.SetBasket(this);
            }

            return result;
        }

        public Task<IEnumerable<T>> PickMany()
        {
            return PickMany(e => true);
        }

        public async Task Remove(Guid id)
        {
            var idFilter = BuildIdFilter(id);

            await _collection.DeleteOneAsync(idFilter);
        }

        public async Task Remove(Expression<Func<T, bool>> expression)
        {
            await _collection.DeleteOneAsync(expression);
        }

        public async Task RemoveMany(params Guid[] ids)
        {
            await RemoveMany(ids as IEnumerable<Guid>);
        }

        public async Task RemoveMany(IEnumerable<Guid> ids)
        {
            var idsFilters = BuildOrIdsFilter(ids);

            await _collection.DeleteManyAsync(idsFilters);
        }

        public async Task RemoveMany(Expression<Func<T, bool>> expression)
        {
            await _collection.DeleteManyAsync(expression);
        }

        public async Task Sync(T entity)
        {
            var idFilter = BuildIdFilter(entity.Id);

            await _collection.ReplaceOneAsync(idFilter, entity);
        }

        private FilterDefinition<T> BuildOrIdsFilter(IEnumerable<Guid> ids)
        {
            var idsFilters = new List<FilterDefinition<T>>();

            foreach (var id in ids)
            {
                var idFilter = BuildIdFilter(id);

                idsFilters.Add(idFilter);
            }

            var orFilter = _filters.Or(idsFilters);

            return orFilter;
        }

        private FilterDefinition<T> BuildIdFilter(Guid id)
        {
            var idFilter = _filters.Eq("_id", id);

            return idFilter;
        }

        private FilterDefinition<T> BuildWhereFilter(Expression<Func<T, bool>> expression)
        {
            var whereFilter = BuildWhereFilter(expression);

            return whereFilter;
        }
    }
}
