using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Project.GsmTree.Dals
{
    public class GsmTreeDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmTree";
            this.Inserts = "[Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Alias], [Remark]";
            this.Selects = "[Unique], [Name], [Alias], [Remark]";
        }

        public GsmTreeDal()
        {
            this.Init();
        }

        public GsmTreeDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}