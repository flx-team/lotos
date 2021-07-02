using System;
using System.Threading.Tasks;

namespace Lotos.Abstractions.Database
{
    public interface IConnection : IDisposable
    {
        Task<IBasket<T>> GetBasket<T>() where T : IEntity<T>;
    }
}
