using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Bf.Esb.Dals
{
    public class ServiceDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Service";
            this.Inserts = "[Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Name], [Filename], [Url], [Contract], [Remark]";
            this.Selects = "[Unique], [Name], [Filename], [Url], [Contract], [Remark]";
        }

        public ServiceDal()
        {
            this.Init();
        }

        public ServiceDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}