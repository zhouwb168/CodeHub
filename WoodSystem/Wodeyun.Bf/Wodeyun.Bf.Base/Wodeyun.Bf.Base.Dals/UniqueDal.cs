using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Base.Dals
{
    public class UniqueDal : BaseDal
    {
        public UniqueDal()
        { }

        public UniqueDal(SqlTransaction transaction)
            : base(transaction)
        { }

        public int GetValueByName(string name)
        {
            string sql = @"update [Unique]
                           set [Value] = [Value] + 1
                           where [Name] = " + name.ToDatabase() + @";
                           select [Value]
                           from [Unique]
                           where [Name] = " + name.ToDatabase() + ";";

            try { return this.Execute.GetValue(sql).ToInt32(); }
            catch
            {
                this.InsertEntity(name, 1);
                return 1;
            }
        }

        public IList GetValuesByNameAndLength(string name, int length)
        {
            string sql = @"update [Unique]
                           set [Value] = [Value] + " + length.ToDatabase() + @"
                           where [Name] = " + name.ToDatabase() + @";
                           select [Value]
                           from [Unique]
                           where [Name] = " + name.ToDatabase() + ";";
            int max;

            try { max = this.Execute.GetValue(sql).ToInt32(); }
            catch
            {
                this.InsertEntity(name, length);
                max = length;
            }

            IList results = new ArrayList();

            for (int i = max; i > max - length; i--)
                results.Add(i);

            return results;
        }

        private int InsertEntity(string name, int value)
        {
            string sql = @"insert into [Unique] ([Name],
                                                 [Value])
                           values (" + name.ToDatabase() + @",
                                   " + value.ToDatabase() + ")";
            return this.Execute.ExecuteNonQuery(sql);
        }
    }
}
