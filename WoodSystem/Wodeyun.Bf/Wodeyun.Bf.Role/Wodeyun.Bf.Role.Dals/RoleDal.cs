using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Bf.Role.Dals
{
    public class RoleDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Role";
            this.Inserts = "[Unique], [Name], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Remark]";
            this.Selects = "[Unique], [Name], [Remark]";
        }

        public RoleDal()
        {
            this.Init();
        }

        public RoleDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}