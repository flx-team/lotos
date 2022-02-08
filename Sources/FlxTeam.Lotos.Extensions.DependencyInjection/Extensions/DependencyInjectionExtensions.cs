using FlxTeam.Lotos.DependencyInjection.Common;
using Microsoft.Extensions.DependencyInjection;

namespace FlxTeam.Lotos.DependencyInjection.Extensions;

public static class LotosDependencyInjectionExtensions
{
    public static LotosBuilder AddLotos(this IServiceCollection services)
    {
        var builder = new LotosBuilder(services);
        builder.Init();

        return builder;
    }
}