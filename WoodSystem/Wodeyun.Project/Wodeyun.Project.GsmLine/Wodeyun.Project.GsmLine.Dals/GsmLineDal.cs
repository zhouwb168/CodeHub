using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Project.GsmLine.Dals
{
    public class GsmLineDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmLine";
            this.Inserts = "[Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Alias], [Remark]";
            this.Selects = "[Unique], [Name], [Alias], [Remark]";
        }

        public GsmLineDal()
        {
            this.Init();
        }

        public GsmLineDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}