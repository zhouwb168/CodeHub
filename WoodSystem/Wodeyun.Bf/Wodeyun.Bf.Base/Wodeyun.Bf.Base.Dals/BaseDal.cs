using System;

using System.Data.SqlClient;

using Wodeyun.Gf.Database.SqlServer;

namespace Wodeyun.Bf.Base.Dals
{
    public class BaseDal : IDisposable
    {
        private Execute _Execute;
        private SqlTransaction _SqlTransaction;

        protected BaseDal()
        {
            this._Execute = new Execute();
        }

        protected BaseDal(string connectionString)
        {
            this._Execute = new Execute(connectionString);
        }

        protected BaseDal(SqlTransaction transaction)
        {
            this._Execute = new Execute(transaction);
        }

        protected Execute Execute
        {
            get { return this._Execute; }
        }

        public SqlTransaction SqlTransaction
        {
            get { return this._SqlTransaction; }
        }

        public void BeginTransaction()
        {
            this._SqlTransaction = this._Execute.BeginTransaction();
        }

        public void CommitTransaction()
        {
            this._SqlTransaction.Commit();
        }

        public void RollbackTransaction()
        {
            this._SqlTransaction.Rollback();
        }

        public string GetFields(string value, string table)
        {
            string result = string.Empty;

            string[] fields = value.Replace(", ", ",").Split(new char[] { ',' });

            for (int i = 0; i < fields.Length; i++)
            {
                string formatter = (i == fields.Length - 1 ? "[{0}].{1}" : "[{0}].{1}, ");
                result = result + string.Format(formatter, table, fields[i]);
            }

            return result;
        }

        public string GetOrders(string value, string table)
        {
            string result = string.Empty;

            string[] orders = value.Replace(", ", ",").Split(new char[] { ',' });

            for (int i = 0; i < orders.Length; i++)
            {
                string formatter = (i == orders.Length - 1 ? "[{0}].{1}" : "[{0}].{1}, ");
                result = result + string.Format(formatter, table, orders[i]);
            }

            return result;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._SqlTransaction != null) this._SqlTransaction.Dispose();
            
            this._Execute.Dispose();
        }

        #endregion
    }
}
