using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FlxTeam.Lotos.Abstractions.Database;
using MongoDB.Driver;

namespace FlxTeam.Lotos.Drivers.Mongo.Database;

internal sealed class MongoBox<TEntity> : IBox<TEntity> where TEntity : Entity<TEntity>
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly FilterDefinitionBuilder<TEntity> _filters;

    public MongoBox(IConnection? connection, IMongoCollection<TEntity> collection)
    {
        _collection = collection;
        Connection = connection;
        _filters = Builders<TEntity>.Filter;
    }
    
    public IConnection? Connection { get; }

    public long Count(Expression<Func<TEntity, bool>> expression)
    {
        var count = _collection.CountDocuments(expression);

        return count;
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        var count = await _collection.CountDocumentsAsync(expression);

        return count;
    }

    public bool Exists(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        var options = new CountOptions { Limit = 1 };

        var count = _collection.CountDocuments(idFilter, options);

        return count != 0;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        var options = new CountOptions { Limit = 1 };

        var count = await _collection.CountDocumentsAsync(idFilter, options);

        return count != 0;
    }

    public bool Exists(Expression<Func<TEntity, bool>> expression)
    {
        var options = new CountOptions { Limit = 1 };

        var count = _collection.CountDocuments(expression, options);

        return count != 0;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        var options = new CountOptions { Limit = 1 };

        var count = await _collection.CountDocumentsAsync(expression, options);

        return count != 0;
    }

    public long Count()
    {
        return Count(e => true);
    }

    public Task<long> CountAsync()
    {
        return CountAsync(e => true);
    }

    public TEntity Put(TEntity entity)
    {
        if (entity.Id == default)
        {
            entity.Id = Guid.NewGuid();
        }

        _collection.InsertOne(entity);

        entity.Basket = this;
        
        return entity;
    }

    public async Task<TEntity> PutAsync(TEntity entity)
    {
        if (entity.Id == default)
        {
            entity.Id = Guid.NewGuid();
        }

        await _collection.InsertOneAsync(entity);

        entity.Basket = this;
        
        return entity;
    }

    public void Sync(TEntity entity)
    {
        var idFilter = BuildIdFilter(entity.Id);

        _collection.ReplaceOne(idFilter, entity);
    }

    public TEntity? Pick(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        var result = _collection.Find(idFilter).FirstOrDefault();
        
        if (result is not null) result.Basket = this;
                
        return result;
    }

    public async Task<TEntity?> PickAsync(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        var result = await _collection.Find(idFilter).FirstOrDefaultAsync();

        if (result is not null) result.Basket = this;
                
        return result;
    }

    public TEntity? Pick(Expression<Func<TEntity, bool>> expression)
    {
        var result = _collection.Find(expression).FirstOrDefault();

         if (result is not null) result.Basket = this;
                
         return result;
    }

    public async Task<TEntity?> PickAsync(Expression<Func<TEntity, bool>> expression)
    {
        var result = await _collection.Find(expression).FirstOrDefaultAsync();
        
        if (result is not null) result.Basket = this;
        
        return result;
    }

    public IEnumerable<TEntity> PickMany()
    {
        return PickMany(e => true);
    }

    public IEnumerable<TEntity> PickMany(params Guid[] ids)
    {
        return PickMany(ids as IEnumerable<Guid>);
    }

    public Task<IEnumerable<TEntity>> PickManyAsync(params Guid[] ids)
    {
        return PickManyAsync(ids as IEnumerable<Guid>);
    }

    public IEnumerable<TEntity> PickMany(IEnumerable<Guid> ids)
    {
        var idsFilter = BuildOrIdsFilter(ids);

        var result = _collection.Find(idsFilter).ToList();

        return result.Select(e =>
        {
            e.Basket = this;
            return e;
        });
    }

    public async Task<IEnumerable<TEntity>> PickManyAsync(IEnumerable<Guid> ids)
    {
        var idsFilter = BuildOrIdsFilter(ids);

        var result = await _collection.Find(idsFilter).ToListAsync();

        return result.Select(e =>
        {
            e.Basket = this;
            return e;
        });
    }

    public IEnumerable<TEntity> PickMany(Expression<Func<TEntity, bool>> expression)
    {
        var result = _collection.Find(expression).ToList();

        return result.Select(e =>
        {
            e.Basket = this;
            return e;
        });
    }

    public async Task<IEnumerable<TEntity>> PickManyAsync(Expression<Func<TEntity, bool>> expression)
    {
        var result = await _collection.Find(expression).ToListAsync();

        return result.Select(e =>
        {
            e.Basket = this;
            return e;
        });
    }

    public void Remove(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        _collection.DeleteOne(idFilter);
    }

    public Task<IEnumerable<TEntity>> PickManyAsync()
    {
        return PickManyAsync(e => true);
    }

    public async Task RemoveAsync(Guid id)
    {
        var idFilter = BuildIdFilter(id);

        await _collection.DeleteOneAsync(idFilter);
    }

    public void Remove(Expression<Func<TEntity, bool>> expression)
    {
        _collection.DeleteOne(expression);
    }

    public async Task RemoveAsync(Expression<Func<TEntity, bool>> expression)
    {
        await _collection.DeleteOneAsync(expression);
    }

    public void RemoveMany()
    {
        RemoveMany(e => true);
    }

    public Task RemoveManyAsync()
    {
        return RemoveManyAsync(e => true);
    }

    public void RemoveMany(params Guid[] ids)
    {
        RemoveMany(ids as IEnumerable<Guid>);
    }

    public async Task RemoveManyAsync(params Guid[] ids)
    {
        await RemoveManyAsync(ids as IEnumerable<Guid>);
    }

    public void RemoveMany(IEnumerable<Guid> ids)
    {
        var idsFilters = BuildOrIdsFilter(ids);

        _collection.DeleteMany(idsFilters);
    }

    public async Task RemoveManyAsync(IEnumerable<Guid> ids)
    {
        var idsFilters = BuildOrIdsFilter(ids);

        await _collection.DeleteManyAsync(idsFilters);
    }

    public void RemoveMany(Expression<Func<TEntity, bool>> expression)
    {
        _collection.DeleteMany(expression);
    }

    public async Task RemoveManyAsync(Expression<Func<TEntity, bool>> expression)
    {
        await _collection.DeleteManyAsync(expression);
    }

    public async Task SyncAsync(TEntity entity)
    {
        var idFilter = BuildIdFilter(entity.Id);

        await _collection.ReplaceOneAsync(idFilter, entity);
    }

    private FilterDefinition<TEntity> BuildOrIdsFilter(IEnumerable<Guid> ids)
    {
        var idsFilters = ids.Select(BuildIdFilter).ToList();

        var orFilter = _filters.Or(idsFilters);

        return orFilter;
    }

    private FilterDefinition<TEntity> BuildIdFilter(Guid id)
    {
        var idFilter = _filters.Eq("_id", id);

        return idFilter;
    }

    public void Dispose() { }
}