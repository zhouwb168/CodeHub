using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Base.Dals
{
    public class CommonDal : BaseDal
    {
        private string _Table = string.Empty;
        private string _Inserts = string.Empty;
        private string _Updates = string.Empty;
        private string _Selects = string.Empty;
        private string _Order = "[Unique]";

        public CommonDal()
        { }

        public CommonDal(SqlTransaction transaction)
            : base(transaction)
        { }

        public string Table
        {
            get { return this._Table; }
            set { this._Table = value; }
        }

        public string Inserts
        {
            set { this._Inserts = value; }
        }

        public string Updates
        {
            set { this._Updates = value; }
        }

        public string Selects
        {
            get { return this._Selects; }
            set { this._Selects = value; }
        }

        public string Order
        {
            set { this._Order = value; }
        }

        public int InsertEntity(Entity entity)
        {
            string sql = @"insert into [" + this._Table + "] (" + this._Inserts + @")
                           values (" + entity.ToInsert(this._Inserts) + ")";
            return this.Execute.ExecuteNonQuery(sql);
        }

        public int UpdateEntityByUnique(Entity entity)
        {
            string sql = @"update [" + this._Table + @"]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase() + @";
                           insert into [" + this._Table + @"]
                           select top 1 " + entity.ToUpdate(this._Inserts, this._Updates).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "[Version] + 1") + @"
                           from [" + this._Table + @"]
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Updated.ToDatabase() + @"
                           order by [Version] desc";
                           
            return this.Execute.ExecuteNonQuery(sql);
        }

        public int DeleteEntityByUnique(int unique)
        {
            string sql = @"update [" + this._Table + @"]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [Unique] = " + unique.ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase();
            return this.Execute.ExecuteNonQuery(sql);
        }

        public Entity GetEntityByField(string field, object value, string connect)
        {
            string sql = @"select " + this._Selects + @"
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect);
            return this.Execute.GetEntity(sql);
        }

        public virtual EntityCollection GetEntities()
        {
            string sql = @"select " + this._Selects + @"
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase();
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdated()
        {
            string sql = @"select " + this._Inserts + @"
                           from [" + this._Table + @"]
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + "]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesByStartAndLength(int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Selects + @"
                               from [" + this._Table + @"]
                               where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase();
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdatedByStartAndLength(int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Inserts + @"
                               from [" + this._Table + @"]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + "]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesByField(string field, object value, string connect)
        {
            string sql = @"select " + this._Selects + @"
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect) + @"
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdatedByField(string field, object value, string connect)
        {
            string sql = @"select " + this._Inserts + @"
                           from [" + this._Table + @"]
                           where " + this.GetWhere(field, value, connect) + @"
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where " + this.GetWhere(field, value, connect);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesByFieldAndStartAndLength(string field, object value, string connect, int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Selects + @"
                               from [" + this._Table + @"]
                               where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and " + this.GetWhere(field, value, connect) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdatedByFieldAndStartAndLength(string field, object value, string connect, int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Inserts + @"
                               from [" + this._Table + @"]
                               where " + this.GetWhere(field, value, connect) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where " + this.GetWhere(field, value, connect);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesByFilter(Entity filter, Entity connector)
        {
            string sql = @"select " + this._Selects + @"
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(filter, connector) + @"
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(filter, connector);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdatedByFilter(Entity filter, Entity connector)
        {
            string sql = @"select " + this._Inserts + @"
                           from [" + this._Table + @"]
                           where " + this.GetWhere(filter, connector) + @"
                           order by " + this._Order + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where " + this.GetWhere(filter, connector);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Selects + @"
                               from [" + this._Table + @"]
                               where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and " + this.GetWhere(filter, connector) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(filter, connector);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public virtual EntityCollection GetEntitiesWithDeletedAndUpdatedByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by " + this._Order + ") as [Number], " + this._Inserts + @"
                               from [" + this._Table + @"]
                               where " + this.GetWhere(filter, connector) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._Table + @"]
                           where " + this.GetWhere(filter, connector);
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public string GetWhere(string field, object value, string connect)
        {
            Entity filter = this.GetFilter(field, value);
            Entity connector = this.GetConnector(field, connect);

            return this.GetWhere(filter, connector);
        }

        private Entity GetFilter(string field, object value)
        {
            PropertyCollection properties = new PropertyCollection();
            properties.Add(new SimpleProperty(field, value.GetType()));

            Entity result = new Entity(properties);
            result.SetValue(field, value);

            return result;
        }

        private Entity GetConnector(string field, string connect)
        {
            PropertyCollection properties = new PropertyCollection();
            properties.Add(new SimpleProperty(field, typeof(string)));

            Entity result = new Entity(properties);
            result.SetValue(field, connect);

            return result;
        }

        public string GetWhere(Entity filter, Entity connector)
        {
            string result = string.Empty;

            for (int i = 0; i < filter.PropertyCollection.Items.Count; i++)
            {
                string field = filter.PropertyCollection[i].Name;
                string value = filter.GetValue(field).ToDatabase();
                string like = filter.GetValue(field).ToLike().ToDatabase();
                string begin = filter.GetValue(field).ToDateBegin();
                string end = filter.GetValue(field).ToDateEnd();
                string connect = connector.GetValue(field).ToString();

                if (connect == "=") result = result + string.Format("[{0}] = {1}", field, value);
                if (connect == ">") result = result + string.Format("[{0}] > {1}", field, value);
                if (connect == "<") result = result + string.Format("[{0}] < {1}", field, value);
                if (connect == ">=") result = result + string.Format("[{0}] >= {1}", field, value);
                if (connect == "<=") result = result + string.Format("[{0}] <= {1}", field, value);
                if (connect == "<>") result = result + string.Format("[{0}] <> {1}", field, value);
                if (connect == "=date") result = result + string.Format("([{0}] >= {1} and [{0}] <= {2})", field, begin, end);
                if (connect == ">date") result = result + string.Format("[{0}] > {1}", field, end);
                if (connect == "<date") result = result + string.Format("[{0}] < {1}", field, begin);
                if (connect == ">=date") result = result + string.Format("[{0}] >= {1}", field, begin);
                if (connect == "<=date") result = result + string.Format("[{0}] <= {1}", field, end);
                if (connect == "<>date") result = result + string.Format("([{0}] > {1} or [{0}] < {2})", field, end, begin);
                if (connect == "null") result = result + string.Format("[{0}] is null", field);
                if (connect == "not null") result = result + string.Format("[{0}] is not null", field);
                if (connect == "like") result = result + string.Format("[{0}] like {1}", field, like);

                if (i != filter.PropertyCollection.Items.Count - 1) result = result + " and ";
            }

            return result;
        }
    }
}
