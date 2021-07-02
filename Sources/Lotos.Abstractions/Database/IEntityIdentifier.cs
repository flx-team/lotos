using System;

namespace Lotos.Abstractions.Database
{
    public interface IEntityIdentifier<T>
    {
        T Id { get; }

        void SetId(T? id);
    }
}
