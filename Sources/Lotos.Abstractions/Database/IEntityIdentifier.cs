using System;

namespace Lotos.Abstractions.Database
{
    public interface IEntityIdentifier
    {
        Guid Id { get; }

        void SetId(Guid id);
    }
}
