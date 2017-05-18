using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Project.GsmSupplier.Dals
{
    public class GsmSupplierDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmSupplier";
            this.Inserts = "[Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log], [LinkMan], [LinkPhone]";
            this.Updates = "[Type], [Name], [Remark], [LinkMan], [LinkPhone]";
            this.Selects = "[Unique], [Type], [Name], [Remark], [LinkMan], [LinkPhone]";
            this.Order = "[Name]";
        }

        public GsmSupplierDal()
        {
            this.Init();
        }

        public GsmSupplierDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}