using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lotos.Abstractions.Database
{
    public interface IBasket<T> where T : IEntity<T>
    {
        Task<T> Keep(T entity);

        Task Sync(T entity);

        Task<T?> Pick(object id);

        Task<T?> Pick(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> PickMany();

        Task<IEnumerable<T>> PickMany(params object[] ids);

        Task<IEnumerable<T>> PickMany(IEnumerable<object> ids);

        Task<IEnumerable<T>> PickMany(Expression<Func<T, bool>> expression);

        Task Remove(object id);

        Task Remove(Expression<Func<T, bool>> expression);

        Task RemoveMany(params object[] ids);

        Task RemoveMany(IEnumerable<object> ids);

        Task RemoveMany(Expression<Func<T, bool>> expression);

        Task<bool> Exists(object id);

        Task<bool> Exists(Expression<Func<T, bool>> expression);

        Task<int> Count(Expression<Func<T, bool>> expression);
    }
}
