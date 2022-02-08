using System.Reflection;
using FlxTeam.Lotos.Abstractions.Attributes;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace FlxTeam.Lotos.Drivers.Mongo.Conventions;

internal class EntityClassMapConvention : ConventionBase, IClassMapConvention
{
    public void Apply(BsonClassMap classMap)
    {
        foreach (var memberMap in classMap.DeclaredMemberMaps)
        {
            if (memberMap.MemberInfo.GetCustomAttribute<IgnoreAttribute>() == null) continue;
            classMap.UnmapMember(memberMap.MemberInfo);
            return;
        }

    }
}