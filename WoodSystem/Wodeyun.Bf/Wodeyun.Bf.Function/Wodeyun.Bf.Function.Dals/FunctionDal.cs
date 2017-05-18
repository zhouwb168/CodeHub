using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Bf.Function.Dals
{
    public class FunctionDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Function";
            this.Inserts = "[Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Parent], [Type], [Order], [No], [Name], [Url], [Remark]";
            this.Selects = "[Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark]";
            this.Order = "[Parent], [Order]";
        }

        public FunctionDal()
        {
            this.Init();
        }

        public FunctionDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

    }
}