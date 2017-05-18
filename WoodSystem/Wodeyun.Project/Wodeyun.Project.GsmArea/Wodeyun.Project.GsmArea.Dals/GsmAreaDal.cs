using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Project.GsmArea.Dals
{
    public class GsmAreaDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmArea";
            this.Inserts = "[Unique], [Name], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Remark]";
            this.Selects = "[Unique], [Name], [Remark]";
        }

        public GsmAreaDal()
        {
            this.Init();
        }

        public GsmAreaDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}