using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FlxTeam.Lotos.Abstractions.Database;

public interface IBox<TEntity> : IDisposable where TEntity : Entity<TEntity>
{
    IConnection? Connection { get; }
    
    TEntity Put(TEntity entity);
    Task<TEntity> PutAsync(TEntity entity);
    
    void Sync(TEntity entity);
    Task SyncAsync(TEntity entity);

    TEntity? Pick(Guid id);
    Task<TEntity?> PickAsync(Guid id);

    TEntity? Pick(Expression<Func<TEntity, bool>> expression);
    Task<TEntity?> PickAsync(Expression<Func<TEntity, bool>> expression);

    IEnumerable<TEntity> PickMany();
    Task<IEnumerable<TEntity>> PickManyAsync();
    
    IEnumerable<TEntity> PickMany(params Guid[] ids);
    Task<IEnumerable<TEntity>> PickManyAsync(params Guid[] ids);

    IEnumerable<TEntity> PickMany(IEnumerable<Guid> ids);
    Task<IEnumerable<TEntity>> PickManyAsync(IEnumerable<Guid> ids);

    IEnumerable<TEntity> PickMany(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> PickManyAsync(Expression<Func<TEntity, bool>> expression);
    
    void Remove(Guid id);
    Task RemoveAsync(Guid id);

    void Remove(Expression<Func<TEntity, bool>> expression);
    Task RemoveAsync(Expression<Func<TEntity, bool>> expression);
    
    void RemoveMany();
    Task RemoveManyAsync();

    void RemoveMany(params Guid[] ids);
    Task RemoveManyAsync(params Guid[] ids);

    void RemoveMany(IEnumerable<Guid> ids);
    Task RemoveManyAsync(IEnumerable<Guid> ids);

    void RemoveMany(Expression<Func<TEntity, bool>> expression);
    Task RemoveManyAsync(Expression<Func<TEntity, bool>> expression);

    bool Exists(Guid id);
    Task<bool> ExistsAsync(Guid id);
    
    bool Exists(Expression<Func<TEntity, bool>> expression);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    
    long Count();
    Task<long> CountAsync();
    
    long Count(Expression<Func<TEntity, bool>> expression);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> expression);
}