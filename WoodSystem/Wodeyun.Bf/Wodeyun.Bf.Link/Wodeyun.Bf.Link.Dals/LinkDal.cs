using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Bf.Link.Dals
{
    public class LinkDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Link";
            this.Inserts = "[Unique], [Name], [Type], [Value], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Type], [Value], [Remark]";
            this.Selects = "[Unique], [Name], [Type], [Value], [Remark]";
        }

        public LinkDal()
        {
            this.Init();
        }

        public LinkDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}