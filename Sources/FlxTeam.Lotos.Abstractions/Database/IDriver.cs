using System.Threading.Tasks;

namespace FlxTeam.Lotos.Abstractions.Database;

public interface IDriver
{
    IConnection CreateConnection();
    Task<IConnection> CreateConnectionAsync();
}