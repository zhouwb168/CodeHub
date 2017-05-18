using System;

using System.Collections;
using System.Data.SqlClient;
using System.Reflection;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Base.Blls
{
    public class CommonBll : IDisposable
    {
        private CommonDal _Dal;

        public CommonDal Dal
        {
            get { return this._Dal; }
            set { this._Dal = value; }
        }

        public Entity SaveEntity(Entity entity)
        {
            try
            {
                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    entity = this.ExecuteForSaveEntityInsert(this._Dal.SqlTransaction, entity);
                    
                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                    entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}");

                    this._Dal.InsertEntity(entity);
                }
                // 修改
                else
                {
                    entity = this.ExecuteForSaveEntityUpdate(this._Dal.SqlTransaction, entity);

                    this._Dal.UpdateEntityByUnique(entity);
                }

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "保存记录成功！", entity.GetValue("Unique").ToString());
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public Entity SaveEntities(object[] entities)
        {
            try
            {
                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                this.ExecuteForSaveEntities(this._Dal.SqlTransaction, entities, unique);

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "保存记录成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public virtual Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.GetEntitiesByField("Name", entity.GetValue("Name"), "=").Count != 0)
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public virtual Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.GetEntitiesByField("Name", entity.GetValue("Name"), "=").Remove("Unique", entity.GetValue("Unique").ToInt32()).Count != 0)
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public virtual void ExecuteForSaveEntities(SqlTransaction transaction, object[] entities, UniqueDal unique)
        {
        }

        public Entity DeleteEntityByUnique(int unique)
        {
            try
            {
                this._Dal.BeginTransaction();

                this.ExecuteForDelete(this._Dal.SqlTransaction, unique);

                this._Dal.DeleteEntityByUnique(unique);

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "删除记录成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public Entity DeleteEntitiesByUniques(int[] uniques)
        {
            try
            {
                this._Dal.BeginTransaction();

                for (int i = 0; i < uniques.Length; i++)
                {
                    this.ExecuteForDelete(this._Dal.SqlTransaction, uniques[i]);

                    this._Dal.DeleteEntityByUnique(uniques[i]);
                }

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "删除记录成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public virtual void ExecuteForDelete(SqlTransaction transaction, int unique)
        {
        }

        public Entity GetEntityByField(string field, object value, string connect)
        {
            return this._Dal.GetEntityByField(field, value, connect);
        }

        public EntityCollection GetEntities()
        {
            return this._Dal.GetEntities();
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdated()
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdated();
        }

        public EntityCollection GetEntitiesByStartAndLength(int start, int length)
        {
            return this._Dal.GetEntitiesByStartAndLength(start, length);
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdatedByStartAndLength(int start, int length)
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdatedByStartAndLength(start, length);
        }

        public EntityCollection GetEntitiesByField(string field, object value, string connect)
        {
            return this._Dal.GetEntitiesByField(field, value, connect);
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdatedByField(string field, object value, string connect)
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdatedByField(field, value, connect);
        }

        public EntityCollection GetEntitiesByFieldAndStartAndLength(string field, object value, string connect, int start, int length)
        {
            return this._Dal.GetEntitiesByFieldAndStartAndLength(field, value, connect, start, length);
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdatedByFieldAndStartAndLength(string field, object value, string connect, int start, int length)
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdatedByFieldAndStartAndLength(field, value, connect, start, length);
        }

        public EntityCollection GetEntitiesByFilter(Entity filter, Entity connector)
        {
            return this._Dal.GetEntitiesByFilter(filter, connector);
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdatedByFilter(Entity filter, Entity connector)
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdatedByFilter(filter, connector);
        }

        public EntityCollection GetEntitiesByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length)
        {
            return this._Dal.GetEntitiesByFilterAndStartAndLength(filter, connector, start, length);
        }

        public EntityCollection GetEntitiesWithDeletedAndUpdatedByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length)
        {
            return this._Dal.GetEntitiesWithDeletedAndUpdatedByFilterAndStartAndLength(filter, connector, start, length);
        }

        public EntityCollection GetGridByMethodAndFieldsAndUnique(string method, object[] args, IList fields, string unique)
        {
            MethodInfo info = this._Dal.GetType().GetMethod(method);
            EntityCollection collection = (EntityCollection)info.Invoke(this._Dal, this.Deserialize(args));

            return this.GetGridByCollectionAndFieldsAndUnique(collection, fields, unique);
        }

        public EntityCollection GetTreeByMethodAndFields(string method, object[] args, IList fields)
        {
            return null;
        }

        public EntityCollection GetGridByMethodAndParent(string method, object[] args, string parent)
        {
            MethodInfo info = this._Dal.GetType().GetMethod(method);
            EntityCollection collection = (EntityCollection)info.Invoke(this._Dal, this.Deserialize(args));

            return this.GetGridByCollectionAndParent(collection, parent);
        }

        public EntityCollection GetTreeByMethodAndParentAndUniqueAndName(string method, object[] args, string parent, string unique, string name)
        {
            MethodInfo info = this._Dal.GetType().GetMethod(method);
            EntityCollection collection = (EntityCollection)info.Invoke(this._Dal, this.Deserialize(args));

            return this.GetTreeByCollectionAndParentAndUniqueAndName(collection, parent, unique, name);
        }

        private object[] Deserialize(object[] args)
        {
            object[] results = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToString().StartsWith("{") == true && args[i].ToString().EndsWith("}") == true)
                    results[i] = Helper.Deserialize(args[i].ToString());
                else
                    results[i] = args[i];
            }

            return results;
        }

        private EntityCollection GetGridByCollectionAndFieldsAndUnique(EntityCollection collection, IList fields, string unique)
        {
            PropertyCollection properties = collection.PropertyCollection;
            properties.Add(new SimpleProperty("_parentId", typeof(int)));

            EntityCollection results = new EntityCollection(properties);
            results.Total = collection.Total;

            EntityCollection returns = this.GetGridChildren(collection, properties, fields, unique, 0, null);

            for (int j = 0; j < returns.Count; j++)
            {
                results.Add(returns[j]);
            }

            return results;
        }

        private EntityCollection GetGridChildren(EntityCollection collection, PropertyCollection properties, IList fields, string unique, int index, Nullable<int> parent)
        {
            EntityCollection results = new EntityCollection(properties);
            
            IList uniques = new ArrayList();
            IList values = new ArrayList();

            for (int i = 0; i < collection.Count; i++)
            {
                int id = collection[i].GetValue(unique).ToInt32() + int.MaxValue / 2 + ((int.MaxValue / 2) / fields.Count) * index;
                object value = collection[i].GetValue(fields[index].ToString());

                if (values.Contains(value) == false)
                {
                    uniques.Add(id);
                    values.Add(value);
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                Entity entity = new Entity(properties);

                entity.SetValue(unique, uniques[i].ToInt32());
                entity.SetValue(fields[index].ToString(), values[i]);
                if(parent.HasValue == true)
                    entity.SetValue("_parentId", parent.Value);
                
                results.Add(entity);

                EntityCollection children = collection.GetEntityCollection(fields[index].ToString(), values[i]);
                EntityCollection returns;

                if (index != fields.Count - 1) returns = this.GetGridChildren(children, properties, fields, unique, index + 1, uniques[i].ToInt32());
                else returns = this.GetGridChildren(children, properties, uniques[i].ToInt32());

                for (int j = 0; j < returns.Count; j++)
                {
                    results.Add(returns[j]);
                }
            }

            return results;
        }

        private EntityCollection GetGridChildren(EntityCollection collection, PropertyCollection properties, int parent)
        {
            EntityCollection results = new EntityCollection(properties);

            for (int i = 0; i < collection.Count; i++)
            {
                Entity entity = new Entity(properties);

                for (int j = 0; j < properties.Count; j++)
                {
                    if (properties[j].Name != "_parentId")
                        entity.SetValue(properties[j].Name, collection[i].GetValue(properties[j].Name));
                }

                entity.SetValue("_parentId", parent);

                results.Add(entity);
            }

            return results;
        }

        private EntityCollection GetGridByCollectionAndParent(EntityCollection collection, string parent)
        {
            PropertyCollection properties = collection.PropertyCollection;
            properties.Add(new SimpleProperty("_parentId", typeof(int)));

            EntityCollection results = new EntityCollection(properties);
            results.Total = collection.Total;

            for (int i = 0; i < collection.Count; i++)
            {
                Entity entity = new Entity(properties);

                for (int j = 0; j < properties.Count; j++)
                {
                    if (properties[j].Name != "_parentId")
                        entity.SetValue(properties[j].Name, collection[i].GetValue(properties[j].Name));
                }

                if (collection[i].GetValue(parent) != null)
                    entity.SetValue("_parentId", collection[i].GetValue(parent));

                results.Add(entity);
            }

            return results;
        }

        private EntityCollection GetTreeByCollectionAndParentAndUniqueAndName(EntityCollection collection, string parent, string unique, string name)
        {
            return this.GetTreeChildren(collection, parent, unique, name, null);
        }

        private EntityCollection GetTreeChildren(EntityCollection collection, string parent, string unique, string name, Nullable<int> id)
        {
            PropertyCollection properties = new PropertyCollection();
            properties.Add(new SimpleProperty("id", typeof(int)));
            properties.Add(new SimpleProperty("text", typeof(string)));
            properties.Add(new SimpleProperty("children", typeof(Entity[])));

            EntityCollection results = new EntityCollection(properties);
            EntityCollection nodes = collection.GetEntityCollection(parent, id);

            for (int i = 0; i < nodes.Count; i++)
            {
                Entity entity = new Entity(properties);
                entity.SetValue("id", nodes[i].GetValue(unique).ToInt32());
                entity.SetValue("text", nodes[i].GetValue(name).ToString());
                entity.SetValue("children", this.GetTreeChildren(collection, parent, unique, name, nodes[i].GetValue(unique).ToInt32()));

                results.Add(entity);
            }

            return results;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._Dal != null) this._Dal.Dispose();
        }

        #endregion
    }
}
