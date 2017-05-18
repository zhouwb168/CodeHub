using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Grant.Dals
{
    public class GrantDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Grant";
            this.Inserts = "[Unique], [Role], [Function], [State], [Version], [Operator], [Log]";
            this.Updates = "[Role], [Function]";
            this.Selects = "[Unique], [Role], [Function]";
        }

        public GrantDal()
        {
            this.Init();
        }

        public GrantDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public int UpdateEntitiesByRole(int role)
        {
            string sql = @"update [" + this.Table + @"]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Role] = " + role.ToDatabase();
            return this.Execute.ExecuteNonQuery(sql);
        }
    }
}