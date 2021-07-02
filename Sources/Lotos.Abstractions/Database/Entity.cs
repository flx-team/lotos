using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Attributes;

namespace Lotos.Abstractions.Database
{
    public class Entity<T> : IEntity<T> where T : IEntity<T>
    {
        public object Id { get; private set; } = null!;

        public IBasket<T> Basket { get; private set; } = null!;

        public async Task Remove()
        {
            await Basket.Remove(Id);
        }

        public async Task<T> Pick()
        {
            return (await Basket.Pick(Id))!;
        }

        public async Task Sync()
        {
            await Basket.Sync((T)(IEntity<T>)this);
        }

        public void SetId(object? id)
        {
            Id = id ?? null!;
        }

        public void SetBasket(IBasket<T>? basket)
        {
            Basket = basket ?? null!;
        }
    }
}
