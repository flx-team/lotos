using System;
using System.Threading.Tasks;

namespace Lotos.Abstractions.Database
{
    public interface IDriver
    {
        Task<IConnection> Run();
    }
}
