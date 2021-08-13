using System;
using System.Reflection;

using Lotos.Abstractions.Attributes;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Lotos.Mongo.Conventions
{
    internal class EntityClassMapConvention : ConventionBase, IClassMapConvention
    {
        public void Apply(BsonClassMap classMap)
        {
            foreach (var memberMap in classMap.DeclaredMemberMaps)
            {
                if (memberMap.MemberInfo.GetCustomAttribute<IgnoreAttribute>() != null)
                {
                    classMap.UnmapMember(memberMap.MemberInfo);
                    return;
                }
            }

        }
    }
}