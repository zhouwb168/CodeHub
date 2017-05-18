using System.Collections;
using System.Data.SqlClient;
using System.Web.Configuration;

using Wodeyun.Gf.Database.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Gf.Database.SqlServer
{
    public class Execute : IExecuteInterface
    {
        private SqlConnection _SqlConnection;
        private SqlTransaction _SqlTransaction;

        public Execute()
        {
            this._SqlConnection = new SqlConnection(this.GetConnectionString("ConnectionString"));

            this._SqlConnection.Open();
        }

        public Execute(string connectionStringName)
        {
            this._SqlConnection = new SqlConnection(this.GetConnectionString(connectionStringName));

            this._SqlConnection.Open();
        }

        public Execute(SqlTransaction transaction)
        {
            this._SqlConnection = transaction.Connection;
            this._SqlTransaction = transaction;
        }

        public SqlTransaction BeginTransaction()
        {
            this._SqlTransaction = this._SqlConnection.BeginTransaction();

            return this._SqlTransaction;
        }

        private string GetConnectionString(string connectionStringName)
        {
            return WebConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        private SqlCommand GetSqlCommand(string sql)
        {
            return new SqlCommand(sql, this._SqlConnection, this._SqlTransaction);
        }

        #region IExecute 成员

        public int ExecuteNonQuery(string sql)
        {
            return this.GetSqlCommand(sql).ExecuteNonQuery();
        }

        public IList GetEntityCollections(string sql, IList propertyCollections)
        {
            IList results = new ArrayList();

            int index = 0;

            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            do
            {
                EntityCollection entityCollection = new EntityCollection(propertyCollections[index] as PropertyCollection);

                while (reader.Read() == true)
                {
                    Entity entity = new Entity(propertyCollections[index] as PropertyCollection);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string[] names = reader.GetName(i).Split(new char[] { '/' });

                        if (names.Length == 1) entity.SetValue(names[0], reader.GetValue(i).ToVariable());
                        else entity.SetValue(names[names.Length - 1], reader.GetValue(i).ToVariable(), names.Left(names.Length - 1));
                    }

                    entityCollection.Add(entity);
                }

                results.Add(entityCollection);

                index = index + 1;
            }
            while (reader.NextResult() == true);

            reader.Close();

            return results;
        }

        public IList GetEntityCollections(string sql)
        {
            IList results = new ArrayList();

            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            do
            {
                PropertyCollection propertyCollection = new PropertyCollection();

                for (int i = 0; i < reader.FieldCount; i++)
                    propertyCollection.Add(new SimpleProperty(reader.GetName(i), reader.GetFieldType(i)));

                EntityCollection entityCollection = new EntityCollection(propertyCollection);

                while (reader.Read() == true)
                {
                    Entity entity = new Entity(propertyCollection);

                    for (int i = 0; i < reader.FieldCount; i++)
                        entity.SetValue(i, reader.GetValue(i).ToVariable());

                    entityCollection.Add(entity);
                }

                results.Add(entityCollection);
            }
            while (reader.NextResult() == true);

            reader.Close();

            return results;
        }

        public EntityCollection GetEntityCollection(string sql, PropertyCollection propertyCollection)
        {
            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            EntityCollection entityCollection = new EntityCollection(propertyCollection);

            while (reader.Read() == true)
            {
                Entity entity = new Entity(propertyCollection);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string[] names = reader.GetName(i).Split(new char[] { '/' });

                    if (names.Length == 1) entity.SetValue(names[0], reader.GetValue(i).ToVariable());
                    else entity.SetValue(names[names.Length - 1], reader.GetValue(i).ToVariable(), names.Left(names.Length - 1));
                }

                entityCollection.Add(entity);
            }

            reader.Close();

            return entityCollection;
        }

        public EntityCollection GetEntityCollection(string sql)
        {
            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            PropertyCollection propertyCollection = new PropertyCollection();

            for (int i = 0; i < reader.FieldCount; i++)
                propertyCollection.Add(new SimpleProperty(reader.GetName(i), reader.GetFieldType(i)));

            EntityCollection entityCollection = new EntityCollection(propertyCollection);

            while (reader.Read() == true)
            {
                Entity entity = new Entity(propertyCollection);

                for (int i = 0; i < reader.FieldCount; i++)
                    entity.SetValue(i, reader.GetValue(i).ToVariable());

                entityCollection.Add(entity);
            }

            reader.Close();

            return entityCollection;
        }

        public Entity GetEntity(string sql, PropertyCollection propertyCollection)
        {
            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            Entity entity = new Entity(propertyCollection);

            while (reader.Read() == true)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string[] names = reader.GetName(i).Split(new char[] { '/' });

                    if (names.Length == 1) entity.SetValue(names[0], reader.GetValue(i).ToVariable());
                    else entity.SetValue(names[names.Length - 1], reader.GetValue(i).ToVariable(), names.Left(names.Length - 1));
                }

                break;
            }

            reader.Close();

            return entity;
        }

        public Entity GetEntity(string sql)
        {
            SqlDataReader reader = this.GetSqlCommand(sql).ExecuteReader();

            PropertyCollection propertyCollection = new PropertyCollection();

            for (int i = 0; i < reader.FieldCount; i++)
                propertyCollection.Add(new SimpleProperty(reader.GetName(i), reader.GetFieldType(i)));

            Entity entity = new Entity(propertyCollection);

            while (reader.Read() == true)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    entity.SetValue(i, reader.GetValue(i).ToVariable());

                break;
            }

            reader.Close();

            return entity;
        }

        public object GetValue(string sql)
        {
            Entity result = this.GetEntity(sql);

            if (result.IsEmpty == true) return null;
            else return result.GetValue(0);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._SqlTransaction != null) this._SqlTransaction.Dispose();

            this._SqlConnection.Close();
            this._SqlConnection.Dispose();
        }

        #endregion
    }
}
