using System;

using System.Collections;

using Wodeyun.Gf.Entities;

namespace Wodeyun.Gf.Database.Interfaces
{
    public interface IExecuteInterface : IDisposable
    {
        int ExecuteNonQuery(string sql);
        IList GetEntityCollections(string sql, IList propertyCollections);
        IList GetEntityCollections(string sql);
        EntityCollection GetEntityCollection(string sql, PropertyCollection propertyCollection);
        EntityCollection GetEntityCollection(string sql);
        Entity GetEntity(string sql, PropertyCollection propertyCollection);
        Entity GetEntity(string sql);
        object GetValue(string sql);
    }
}
