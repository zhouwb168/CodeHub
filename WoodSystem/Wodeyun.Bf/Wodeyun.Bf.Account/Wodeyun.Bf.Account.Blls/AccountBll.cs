using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Account.Dals;
using Wodeyun.Bf.Account.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Account.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AccountBll : CommonBll, IAccountInterface
    {
        private AccountDal _Dal = new AccountDal();

        public AccountBll()
        {
            this.Dal = this._Dal;
        }

        public override Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.GetEntitiesByField("Id", entity.GetValue("Id"), "=").Count != 0)
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public override Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.GetEntitiesByField("Id", entity.GetValue("Id"), "=").Remove("Unique", entity.GetValue("Unique").ToInt32()).Count != 0)
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }
    }
}
