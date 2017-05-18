using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Link.Dals;
using Wodeyun.Bf.Link.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Link.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LinkBll : CommonBll, ILinkInterface
    {
        private LinkDal _Dal = new LinkDal();

        public LinkBll()
        {
            this.Dal = this._Dal;
        }

        public override Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Value").ToString() == string.Empty) entity.SetValue("Value", null);
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public override Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Value").ToString() == string.Empty) entity.SetValue("Value", null);
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }
    }
}