using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Attributes;

namespace Lotos.Abstractions.Database
{
    public interface IEntityProvider<T> where T : IEntity<T>
    {
        IBasket<T> Basket { get; }

        Task Sync();
        Task<T> Pick();
        Task Remove();

        void SetBasket(IBasket<T>? basket);
    }
}
