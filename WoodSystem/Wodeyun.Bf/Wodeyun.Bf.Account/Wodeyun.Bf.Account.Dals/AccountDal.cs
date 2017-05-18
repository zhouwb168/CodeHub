using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;

namespace Wodeyun.Bf.Account.Dals
{
    public class AccountDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Account";
            this.Inserts = "[Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log]";
            this.Updates = "[Id], [Description], [Remark], [Status]";
            this.Selects = "[Unique], [Id], [Description], [Remark], [Status]";
        }

        public AccountDal()
        {
            this.Init();
        }

        public AccountDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}